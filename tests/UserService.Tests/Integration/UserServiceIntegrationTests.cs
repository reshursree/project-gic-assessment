using Microsoft.AspNetCore.Mvc.Testing;
using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using UserService.DTOs;

namespace UserService.Tests.Integration;

public class UserServiceIntegrationTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient _client;

    public UserServiceIntegrationTests(WebApplicationFactory<Program> factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task CreateUser_ReturnsCreatedStatus_And_Response()
    {
        // Arrange
        var request = new CreateUserRequest
        {
            Name = "Refactored User",
            Email = "refactor@example.com"
        };

        // Act
        var response = await _client.PostAsJsonAsync("/api/v1/users", request);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Created);
        
        var result = await response.Content.ReadFromJsonAsync<UserResponse>();
        result.Should().NotBeNull();
        result!.Name.Should().Be(request.Name);
        result.Email.Should().Be(request.Email);
        result.Id.Should().NotBeEmpty();
    }

    [Fact]
    public async Task GetUser_WhenUserExists_ReturnsOkWithUser()
    {
        // Arrange - Create a user first
        var createRequest = new CreateUserRequest
        {
            Name = "Test User",
            Email = "test@example.com"
        };
        var createResponse = await _client.PostAsJsonAsync("/api/v1/users", createRequest);
        var createdUser = await createResponse.Content.ReadFromJsonAsync<UserResponse>();

        // Act - Get the user
        var response = await _client.GetAsync($"/api/v1/users/{createdUser!.Id}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        
        var result = await response.Content.ReadFromJsonAsync<UserResponse>();
        result.Should().NotBeNull();
        result!.Id.Should().Be(createdUser.Id);
        result.Name.Should().Be(createRequest.Name);
        result.Email.Should().Be(createRequest.Email);
    }

    [Fact]
    public async Task GetUser_WhenUserDoesNotExist_ReturnsNotFound()
    {
        // Arrange
        var nonExistentId = Guid.NewGuid();

        // Act
        var response = await _client.GetAsync($"/api/v1/users/{nonExistentId}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task CreateUser_WithDuplicateEmail_ReturnsBadRequest()
    {
        // Arrange
        var request = new CreateUserRequest
        {
            Name = "First User",
            Email = "duplicate@example.com"
        };
        
        // Act - Create user first time
        var firstResponse = await _client.PostAsJsonAsync("/api/v1/users", request);
        firstResponse.StatusCode.Should().Be(HttpStatusCode.Created);

        // Act - Try to create with same email
        var duplicateRequest = new CreateUserRequest
        {
            Name = "Second User",
            Email = "duplicate@example.com" // Same email
        };
        var secondResponse = await _client.PostAsJsonAsync("/api/v1/users", duplicateRequest);

        // Assert - Should prevent duplicate
        secondResponse.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }
}
