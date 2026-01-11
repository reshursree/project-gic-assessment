namespace Shared.Messaging;

/// <summary>
/// Defines a resilient contract for publishing integration events to Kafka topics.
/// </summary>
public interface IKafkaProducer
{
    /// <summary>
    /// Publishes an integration event to a specified topic with retry logic.
    /// </summary>
    /// <typeparam name="T">The type of integration event.</typeparam>
    /// <param name="topic">The Kafka topic to publish to.</param>
    /// <param name="event">The event payload.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    Task PublishAsync<T>(string topic, T @event, CancellationToken cancellationToken = default) 
        where T : IntegrationEvent;
}
