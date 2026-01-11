using Confluent.Kafka;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using System.Text.Json;
using Shared.Messaging;

namespace OrderService.BackgroundServices;

/// <summary>
/// A resilient Kafka consumer that processes user registration events to build local caches or trigger order workflows.
/// </summary>
public class UserCreatedConsumer : BackgroundService
{
    private readonly IConsumer<string, string> _consumer;
    private readonly ILogger<UserCreatedConsumer> _logger;
    private const string Topic = "user-created";

    public UserCreatedConsumer(IConfiguration configuration, ILogger<UserCreatedConsumer> logger)
    {
        _logger = logger;
        
        var config = new ConsumerConfig
        {
            BootstrapServers = configuration["Kafka:BootstrapServers"] ?? configuration["Kafka__BootstrapServers"] ?? "localhost:9092",
            GroupId = "order-service-group",
            AutoOffsetReset = AutoOffsetReset.Earliest,
            EnableAutoCommit = false // Industry standard: Manual commit after processing
        };

        _consumer = new ConsumerBuilder<string, string>(config).Build();
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _consumer.Subscribe(Topic);
        _logger.LogInformation("Started consuming from {Topic}", Topic);

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                // Use timeout instead of cancellation token to avoid interfering with Kestrel startup
                var consumeResult = _consumer.Consume(TimeSpan.FromMilliseconds(100));
                
                if (consumeResult?.Message != null)
                {
                    _logger.LogInformation("Received event: {Key}", consumeResult.Message.Key);
                    
                    // TODO: Process the event (e.g., save to local DB, notify other systems)
                    // var userEvent = JsonSerializer.Deserialize<UserCreatedEvent>(consumeResult.Message.Value);
                    
                    // Industry standard: Commit only after successful processing
                    _consumer.Commit(consumeResult);
                }
                
                // Small delay to prevent tight loop and reduce CPU usage
                await Task.Delay(10, stoppingToken);
            }
            catch (OperationCanceledException)
            {
                break;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error processing Kafka message from {Topic}", Topic);
                // In a production scenario, we'd use a Dead Letter Queue (DLQ) here
                await Task.Delay(1000, stoppingToken); // Back off on error
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
