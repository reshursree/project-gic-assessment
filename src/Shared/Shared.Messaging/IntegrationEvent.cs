namespace Shared.Messaging;

/// <summary>
/// A marker interface for all integration events.
/// Provides base metadata for auditing and tracing.
/// </summary>
public abstract record IntegrationEvent
{
    public Guid EventId { get; init; } = Guid.NewGuid();
    public DateTime OccurredOn { get; init; } = DateTime.UtcNow;
}
