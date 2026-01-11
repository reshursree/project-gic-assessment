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
}
