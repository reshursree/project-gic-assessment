using System.ComponentModel.DataAnnotations;

namespace OrderService.DTOs;

/// <summary>
/// Data transfer object for order creation requests.
/// </summary>
public record CreateOrderRequest
{
    /// <summary>
    /// The unique identifier of the user placing the order.
    /// </summary>
    [Required]
    public Guid UserId { get; init; }

    /// <summary>
    /// The name of the product being ordered.
    /// </summary>
    [Required]
    [StringLength(200)]
    public string Product { get; init; } = string.Empty;

    /// <summary>
    /// The quantity of the product.
    /// </summary>
    [Range(1, 1000)]
    public int Quantity { get; init; }

    /// <summary>
    /// The total price of the order.
    /// </summary>
    [Range(0.01, 1000000.00)]
    public decimal Price { get; init; }
}

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
