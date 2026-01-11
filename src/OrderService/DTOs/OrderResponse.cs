namespace OrderService.DTOs;

/// <summary>
/// Data transfer object representing a successful order details response.
/// </summary>
public record OrderResponse
{
    public Guid Id { get; init; }
    public Guid UserId { get; init; }
    public string Product { get; init; } = string.Empty;
    public int Quantity { get; init; }
    public decimal Price { get; init; }
    public DateTime CreatedAt { get; init; }
}
