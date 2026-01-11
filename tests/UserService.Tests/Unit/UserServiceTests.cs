using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using UserService.Data;
using UserService.DTOs;
using UserService.Services;

namespace UserService.Tests.Unit;

public class UserServiceTests : IDisposable
{
    private readonly UserDbContext _context;
    private readonly IUserService _service;

    public UserServiceTests()
    {
        var options = new DbContextOptionsBuilder<UserDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        _context = new UserDbContext(options);
        _service = new UserService.Services.UserService(_context);
    }

    [Fact]
    public async Task CreateUserAsync_SavesToDatabase()
    {
        // Arrange
        var request = new CreateUserRequest
        {
            Name = "Database User",
            Email = "db@example.com"
        };

        // Act
        var result = await _service.CreateUserAsync(request);

        // Assert
        result.Should().NotBeNull();
        
        // Verify it exists in DB
        var userInDb = await _context.Users.FindAsync(result.Id);
        userInDb.Should().NotBeNull();
        userInDb!.Name.Should().Be(request.Name);
    }

    [Fact]
    public async Task CreateUserAsync_MapsRequestToResponse()
    {
        // Arrange
        var request = new CreateUserRequest
        {
            Name = "Mapping Test",
            Email = "map@example.com"
        };

        // Act
        var result = await _service.CreateUserAsync(request);

        // Assert
        result.Name.Should().Be(request.Name);
        result.Email.Should().Be(request.Email);
    }

    public void Dispose()
    {
        _context.Database.EnsureDeleted();
        _context.Dispose();
    }
}
