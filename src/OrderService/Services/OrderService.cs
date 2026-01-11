using OrderService.Data;
using OrderService.DTOs;
using OrderService.Models;
using Shared.Messaging;
using Shared.Messaging.Events;
using Microsoft.EntityFrameworkCore;

namespace OrderService.Services;

/// <summary>
/// Interface for order management business logic.
/// </summary>
public interface IOrderService
{
    Task<OrderResponse> CreateOrderAsync(CreateOrderRequest request);
    Task<OrderResponse?> GetOrderByIdAsync(Guid id);
}

/// <summary>
/// Implementation of order management business logic.
/// Handles persistence and event publication.
/// </summary>
public class OrderServiceImplementation : IOrderService
{
    private readonly OrderDbContext _context;
    private readonly IKafkaProducer _producer;
    private readonly ILogger<OrderServiceImplementation> _logger;

    public OrderServiceImplementation(
        OrderDbContext context, 
        IKafkaProducer producer, 
        ILogger<OrderServiceImplementation> logger)
    {
        _context = context;
        _producer = producer;
        _logger = logger;
    }

    public async Task<OrderResponse> CreateOrderAsync(CreateOrderRequest request)
    {
        _logger.LogInformation("Creating order for user {UserId}", request.UserId);

        var order = new Order
        {
            Id = Guid.NewGuid(),
            UserId = request.UserId,
            Product = request.Product,
            Quantity = request.Quantity,
            Price = request.Price,
            CreatedAt = DateTime.UtcNow
        };

        _context.Orders.Add(order);
        await _context.SaveChangesAsync();

        // Publish OrderCreatedEvent
        var integrationEvent = new OrderCreatedEvent
        {
            OrderId = order.Id,
            UserId = order.UserId,
            Product = order.Product,
            Quantity = order.Quantity,
            Price = order.Price
        };

        await _producer.PublishAsync("order-created", integrationEvent);
        
        _logger.LogInformation("Order {OrderId} created and event published", order.Id);

        return MapToResponse(order);
    }

    public async Task<OrderResponse?> GetOrderByIdAsync(Guid id)
    {
        var order = await _context.Orders.FirstOrDefaultAsync(o => o.Id == id);
        return order != null ? MapToResponse(order) : null;
    }

    private static OrderResponse MapToResponse(Order order) => new()
    {
        Id = order.Id,
        UserId = order.UserId,
        Product = order.Product,
        Quantity = order.Quantity,
        Price = order.Price,
        CreatedAt = order.CreatedAt
    };
}
