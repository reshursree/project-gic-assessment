namespace OrderService.Models;

/// <summary>
/// Internal domain model representing an Order entity.
/// </summary>
public class Order
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public string Product { get; set; } = string.Empty;
    public int Quantity { get; set; }
    public decimal Price { get; set; }
    public DateTime CreatedAt { get; set; }
}
