using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
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
        var mockLogger = new Mock<ILogger<UserService.Services.UserService>>();
        _service = new UserService.Services.UserService(_context, mockLogger.Object);
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

    [Fact]
    public async Task GetUserByIdAsync_WhenUserExists_ReturnsUser()
    {
        // Arrange
        var createRequest = new CreateUserRequest
        {
            Name = "Existing User",
            Email = "exists@example.com"
        };
        var created = await _service.CreateUserAsync(createRequest);

        // Act
        var result = await _service.GetUserByIdAsync(created.Id);

        // Assert
        result.Should().NotBeNull();
        result!.Id.Should().Be(created.Id);
        result.Name.Should().Be(createRequest.Name);
        result.Email.Should().Be(createRequest.Email);
    }

    [Fact]
    public async Task GetUserByIdAsync_WhenUserDoesNotExist_ReturnsNull()
    {
        // Arrange
        var nonExistentId = Guid.NewGuid();

        // Act
        var result = await _service.GetUserByIdAsync(nonExistentId);

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public async Task CreateUserAsync_WithDuplicateEmail_ThrowsException()
    {
        // Arrange
        var request1 = new CreateUserRequest
        {
            Name = "First User",
            Email = "same@example.com"
        };
        var request2 = new CreateUserRequest
        {
            Name = "Second User",
            Email = "same@example.com" // Duplicate
        };

        await _service.CreateUserAsync(request1);

        // Act & Assert
        var act = async () => await _service.CreateUserAsync(request2);
        await act.Should().ThrowAsync<InvalidOperationException>()
            .WithMessage("*email*already*");
    }

    public void Dispose()
    {
        _context.Database.EnsureDeleted();
        _context.Dispose();
    }
}
