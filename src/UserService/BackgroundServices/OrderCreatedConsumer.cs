using Confluent.Kafka;
using Shared.Messaging.Events;
using System.Text.Json;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace UserService.BackgroundServices;

/// <summary>
/// Demonstrates bidirectional communication by consuming OrderCreatedEvents.
/// In a real system, this could be used for updating user loyalty points or analytics.
/// For this assessment, we log the received events to prove connectivity.
/// </summary>
public class OrderCreatedConsumer : BackgroundService
{
    private readonly IConfiguration _configuration;
    private readonly ILogger<OrderCreatedConsumer> _logger;
    private readonly IConsumer<string, string> _consumer;
    private const string Topic = "order-created";

    public OrderCreatedConsumer(IConfiguration configuration, ILogger<OrderCreatedConsumer> logger)
    {
        _configuration = configuration;
        _logger = logger;

        var config = new ConsumerConfig
        {
            BootstrapServers = configuration["Kafka:BootstrapServers"] ?? configuration["Kafka__BootstrapServers"] ?? "localhost:9092",
            GroupId = "user-service-order-monitor-group",
            AutoOffsetReset = AutoOffsetReset.Earliest,
            EnableAutoCommit = false
        };

        _consumer = new ConsumerBuilder<string, string>(config).Build();
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("UserService starting to monitor {Topic}", Topic);
        _consumer.Subscribe(Topic);

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                var consumeResult = _consumer.Consume(TimeSpan.FromMilliseconds(100));

                if (consumeResult?.Message != null)
                {
                    _logger.LogInformation("UserService caught OrderCreated message: {Message}", consumeResult.Message.Value);
                    
                    var orderEvent = JsonSerializer.Deserialize<OrderCreatedEvent>(consumeResult.Message.Value);
                    if (orderEvent != null)
                    {
                        _logger.LogInformation("Successfully parsed OrderCreatedEvent for Order: {OrderId}, User: {UserId}", orderEvent.OrderId, orderEvent.UserId);
                    }

                    _consumer.Commit(consumeResult);
                }

                await Task.Delay(10, stoppingToken);
            }
            catch (OperationCanceledException)
            {
                break;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error processing OrderCreated message in UserService");
                await Task.Delay(1000, stoppingToken);
            }
        }
    }

    public override void Dispose()
    {
        _consumer.Close();
        _consumer.Dispose();
        base.Dispose();
    }
}
