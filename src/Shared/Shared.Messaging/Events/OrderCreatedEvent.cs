using Shared.Messaging;

namespace Shared.Messaging.Events;

/// <summary>
/// Integration event published when a new order is successfully created.
/// </summary>
public record OrderCreatedEvent : IntegrationEvent
{
    public Guid OrderId { get; init; }
    public Guid UserId { get; init; }
    public string Product { get; init; } = string.Empty;
    public int Quantity { get; init; }
    public decimal Price { get; init; }
}
