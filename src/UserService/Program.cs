using Microsoft.EntityFrameworkCore;
using UserService.Data;
using Asp.Versioning;
using Serilog;
using Serilog.Formatting.Compact;

var builder = WebApplication.CreateBuilder(args);

// Configure Serilog
Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .Enrich.FromLogContext()
    .Enrich.WithProperty("Application", "UserService")
    .WriteTo.Console(new CompactJsonFormatter())
    .CreateLogger();

try
{
    Log.Information("Starting UserService");

// Add Serilog
builder.Host.UseSerilog();

// Add services to the container.
builder.Services.AddDbContext<UserDbContext>(options =>
    options.UseInMemoryDatabase("UserDb"));

// Add Health Checks
builder.Services.AddHealthChecks()
    .AddDbContextCheck<UserDbContext>("database");

// Add API Versioning
builder.Services.AddApiVersioning(options =>
{
    options.DefaultApiVersion = new ApiVersion(1, 0);
    options.AssumeDefaultVersionWhenUnspecified = true;
    options.ReportApiVersions = true;
    options.ApiVersionReader = ApiVersionReader.Combine(
        new UrlSegmentApiVersionReader(),
        new HeaderApiVersionReader("X-Api-Version"));
}).AddApiExplorer(options =>
{
    options.GroupNameFormat = "'v'VVV";
    options.SubstituteApiVersionInUrl = true;
});

builder.Services.AddControllers();
builder.Services.AddScoped<UserService.Services.IUserService, UserService.Services.UserService>();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Title = "User Service API",
        Version = "v1",
        Description = "Handles user registrations and profile management."
    });

    // Enable Swagger Annotations
    options.EnableAnnotations();

    // Senior Signal: Include XML Comments in Swagger
    var xmlFile = $"{System.Reflection.Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    options.IncludeXmlComments(xmlPath);
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "User Service API v1");
    });
}

app.UseHttpsRedirection();

app.UseAuthorization();

// Map Health Check Endpoint
app.MapHealthChecks("/health");

app.MapControllers();

app.Run();

}
catch (Exception ex)
{
    Log.Fatal(ex, "UserService terminated unexpectedly");
    throw;
}
finally
{
    Log.Information("Shutting down UserService");
    Log.CloseAndFlush();
}

// Make the implicit Program class public for test projects
public partial class Program { }
