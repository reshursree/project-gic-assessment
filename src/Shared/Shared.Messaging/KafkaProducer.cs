using Confluent.Kafka;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using Polly;
using Polly.Retry;
using System.Text.Json;

namespace Shared.Messaging;

/// <summary>
/// High-performance Kafka producer with integrated Polly resilience for transient failure handling.
/// </summary>
public class KafkaProducer : IKafkaProducer, IDisposable
{
    private readonly IProducer<string, string> _producer;
    private readonly ILogger<KafkaProducer> _logger;
    private readonly AsyncRetryPolicy _retryPolicy;

    public KafkaProducer(IConfiguration configuration, ILogger<KafkaProducer> logger)
    {
        _logger = logger;
        
        var producerConfig = new ProducerConfig
        {
            BootstrapServers = configuration["Kafka:BootstrapServers"] ?? "localhost:9092",
            Acks = Acks.All, // Ensure strong durability
            EnableIdempotence = true, // Ensure exactly-once semantics
            MessageSendMaxRetries = 3
        };

        _producer = new ProducerBuilder<string, string>(producerConfig).Build();

        // Industry standard: Exponential backoff for transient Kafka connection issues
        _retryPolicy = Policy
            .Handle<KafkaException>()
            .WaitAndRetryAsync(3, retryAttempt => 
                TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)),
                (ex, time) => _logger.LogWarning(ex, "Kafka publish failed. Retrying in {Time}...", time));
    }

    public async Task PublishAsync<T>(string topic, T @event, CancellationToken cancellationToken = default) 
        where T : IntegrationEvent
    {
        var payload = JsonSerializer.Serialize(@event);
        var message = new Message<string, string> 
        { 
            Key = @event.EventId.ToString(), 
            Value = payload 
        };

        await _retryPolicy.ExecuteAsync(async () =>
        {
            var result = await _producer.ProduceAsync(topic, message, cancellationToken);
            
            if (result.Status == PersistenceStatus.Persisted)
            {
                _logger.LogInformation("Successfully published {EventType} to {Topic} [Offset: {Offset}]", 
                    typeof(T).Name, topic, result.Offset);
            }
            else
            {
                _logger.LogError("Failed to verify persistence for {EventType} in {Topic}. Status: {Status}", 
                    typeof(T).Name, topic, result.Status);
                throw new KafkaException(new Error(ErrorCode.Unknown, "Persistence not confirmed"));
            }
        });
    }

    public void Dispose()
    {
        _producer?.Flush(TimeSpan.FromSeconds(10));
        _producer?.Dispose();
    }
}
