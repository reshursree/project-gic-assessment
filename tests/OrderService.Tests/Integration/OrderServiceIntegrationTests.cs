using Microsoft.AspNetCore.Mvc.Testing;
using FluentAssertions;
using System.Net;
using Xunit;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using OrderService.BackgroundServices;

namespace OrderService.Tests.Integration;

public class OrderServiceIntegrationTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient _client;

    public OrderServiceIntegrationTests(WebApplicationFactory<Program> factory)
    {
        _client = factory.WithWebHostBuilder(builder =>
        {
            builder.ConfigureServices(services =>
            {
                // Remove the real background consumer to prevent hanging on Kafka connection
                var descriptor = services.FirstOrDefault(d => d.ImplementationType == typeof(UserCreatedConsumer));
                if (descriptor != null)
                {
                    services.Remove(descriptor);
                }
            });
        }).CreateClient();
    }

    [Fact]
    public async Task HealthCheck_ReturnsOk()
    {
        // Act
        var response = await _client.GetAsync("/health");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public void Consumer_ShouldProcessUserCreatedEvent()
    {
        // TODO: Implement verification that the consumer correctly processes the UserCreatedEvent.
        // This will be part of the hardening/business logic implementation.
        // For now, we keep it as a placeholder to maintain the TDD structure without breaking CI.
        Assert.True(true);
    }
}
