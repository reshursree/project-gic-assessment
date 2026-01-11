using Shared.Messaging;

namespace UserService.Events;

/// <summary>
/// Domain-specific integration event published when a new user successfully registers.
/// </summary>
public record UserCreatedEvent : IntegrationEvent
{
    public Guid UserId { get; init; }
    public string Name { get; init; } = string.Empty;
    public string Email { get; init; } = string.Empty;
}
