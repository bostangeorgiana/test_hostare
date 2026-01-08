using CampusEats.Features.Orders.GetActiveOrders;
using CampusEats.Persistence.Context;
using CampusEats.Persistence.Entities;

namespace CampusEats.Features.Orders;

public class OrderRepository : IOrderRepository
{
    private readonly CampusEatsDbContext _context;

    public OrderRepository(CampusEatsDbContext context)
    {
        _context = context;
    }

    public async Task<int> CreateOrderAsync(int studentId, decimal totalAmount, string status, string? notes, CancellationToken ct)
    {
        var order = new Order
        {
            StudentId = studentId,
            TotalAmount = totalAmount,
            Status = status,
            Notes = notes 
        };

        _context.Orders.Add(order);
        await _context.SaveChangesAsync(ct);

        return order.OrderId;
    }

    public async Task AddOrderItemsAsync(int orderId, List<(int MenuItemId, int Quantity, decimal Price)> items, CancellationToken ct)
    {
        var orderItems = items.Select(i => new OrderItem
        {
            OrderId = orderId,
            MenuItemId = i.MenuItemId,
            Quantity = i.Quantity,
            Price = i.Price
        }).ToList();

        _context.OrderItems.AddRange(orderItems);
        await _context.SaveChangesAsync(ct);
    }

    public async Task AddOrderNotesAsync(int orderId, string? notes, CancellationToken ct)
    {
        var order = await _context.Orders.FindAsync(new object[] { orderId }, ct);
        if (order != null)
        {
            order.Notes = notes;
            await _context.SaveChangesAsync(ct);
        }
    }

    public async Task<bool> CancelOrderAsync(int orderId, int studentId, CancellationToken ct)
    {
        throw new NotImplementedException();
    }

    public async Task<List<ActiveOrderDto>> GetActiveKitchenOrdersAsync(CancellationToken ct)
    {
        throw new NotImplementedException();
    }

    public async Task<Order?> GetBasicOrderAsync(int orderId, CancellationToken ct)
    {
        throw new NotImplementedException();
    }

    public async Task<OrderDetailsDto?> GetOrderDetailsAsync(int orderId, CancellationToken ct)
    {
        throw new NotImplementedException();
    }

    public async Task<List<OrderSummaryDto>> GetOrderHistoryAsync(int studentId, CancellationToken ct)
    {
        throw new NotImplementedException();
    }

    public async Task<bool> UpdateOrderStatusAsync(int orderId, string newStatus, int staffUserId, CancellationToken ct)
    {
        throw new NotImplementedException();
    }

    public async Task UpdateOrderStatusDirectAsync(Order order)
    {
        throw new NotImplementedException();
    }
}
