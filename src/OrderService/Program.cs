using OrderService.BackgroundServices;
using OrderService.Data;
using OrderService.Services;
using Shared.Messaging;
using Serilog;
using Serilog.Formatting.Compact;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Configure Serilog
Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .Enrich.FromLogContext()
    .WriteTo.Console(new CompactJsonFormatter())
    .CreateLogger();

builder.Host.UseSerilog();

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
});

// Register EF Core In-Memory Database
builder.Services.AddDbContext<OrderDbContext>(options =>
    options.UseInMemoryDatabase("OrdersDb"));

// Register Business Logic Services
builder.Services.AddScoped<IOrderService, OrderServiceImplementation>();

// Register Messaging
builder.Services.AddSingleton<IKafkaProducer, KafkaProducer>();

// Register the Kafka Consumer as a background service
builder.Services.AddHostedService<UserCreatedConsumer>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.MapGet("/health", () => Results.Ok(new { Status = "Healthy", Service = "OrderService" }))
   .WithName("HealthCheck");

app.Run();

public partial class Program { }
