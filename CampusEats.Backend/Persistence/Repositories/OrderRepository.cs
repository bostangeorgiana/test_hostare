using CampusEats.Features.Orders;
using CampusEats.Features.Orders.GetActiveOrders;
using CampusEats.Persistence.Context;
using CampusEats.Persistence.Entities;
using Microsoft.EntityFrameworkCore;
using CampusEats.Features.Orders.GetActiveOrders;
using Microsoft.Extensions.Logging;

namespace CampusEats.Persistence.Repositories;

public class OrderRepository : IOrderRepository
{
    private readonly CampusEatsDbContext db;
    private readonly ILogger<OrderRepository> logger;

    public OrderRepository(CampusEatsDbContext db, ILogger<OrderRepository> logger)
    {
        this.db = db;
        this.logger = logger;
    }

    public async Task<int> CreateOrderAsync(
        int studentId,
        decimal totalAmount,
        string status,
        string? notes,
        CancellationToken ct)
    {
        
        int pointsEarned = (int)Math.Floor(totalAmount / 10m);
        
        var entity = new Order
        {
            StudentId = studentId,
            TotalAmount = totalAmount,
            Status = status,
            Notes = notes, 
            LoyaltyPointsUsed = 0,
            TotalLoyaltyPointsEarned = pointsEarned,
            FinalAmountPaid = totalAmount,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        db.Orders.Add(entity);
        await db.SaveChangesAsync(ct);

        return entity.OrderId;
    }
    
    public async Task AddOrderItemsAsync(
        int orderId,
        List<(int MenuItemId, int Quantity, decimal Price)> items,
        CancellationToken ct)
    {
        foreach (var item in items)
        {
            db.OrderItems.Add(new OrderItem
            {
                OrderId = orderId,
                MenuItemId = item.MenuItemId,
                Quantity = item.Quantity,
                Price = item.Price
            });
        }

        await db.SaveChangesAsync(ct);
    }
    
    public async Task<List<OrderSummaryDto>> GetOrderHistoryAsync(
        int studentId,
        CancellationToken ct)
    {
        return await db.Orders
            .Where(o => o.StudentId == studentId)
            .OrderByDescending(o => o.CreatedAt)
            .Select(o => new OrderSummaryDto(
                o.OrderId,
                o.TotalAmount,
                o.Status,
                o.CreatedAt,
                o.Notes
            ))
            .ToListAsync(ct);
    }
    
    public async Task<int> GetTodayOrdersCountAsync(CancellationToken ct = default)
    {
        var cutoff = DateTime.UtcNow.AddHours(-24);
    
        var count = await db.Orders
            .Where(o => o.CreatedAt >= cutoff)
            .CountAsync(ct);
    
        Console.WriteLine($"🔍 Orders in last 24h: {count}");
    
        return count;
    }
    
    public async Task<OrderDetailsDto?> GetOrderDetailsAsync(
        int orderId,
        CancellationToken ct)
    {
        var order = await db.Orders
            .Where(o => o.OrderId == orderId)
            .Select(o => new OrderDetailsDto(
                o.OrderId,
                o.TotalAmount,
                o.Status,
                o.CreatedAt,
                o.OrderItems.Select(i => new OrderItemDetailsDto(
                    i.MenuItemId,
                    i.MenuItem.Name,
                    i.Quantity,
                    i.Price
                )).ToList(),
                o.LoyaltyPointsUsed,
                o.TotalLoyaltyPointsEarned,
                o.FinalAmountPaid
            ))
            .FirstOrDefaultAsync(ct);

        return order;
    }
    
    public async Task<bool> CancelOrderAsync(int orderId, int studentId, CancellationToken ct)
    {
        var order = await db.Orders
            .FirstOrDefaultAsync(o => o.OrderId == orderId && o.StudentId == studentId, ct);

        if (order is null)
            throw new Exception("Order not found.");

        if (order.Status != "pending")
            throw new Exception("Only pending orders can be cancelled.");

        order.Status = "cancelled";
        order.UpdatedAt = DateTime.UtcNow;

        await db.SaveChangesAsync(ct);

        return true;
    }
    
    public async Task<bool> UpdateOrderStatusAsync(
        int orderId,
        string newStatus,
        int staffUserId,
        CancellationToken ct)
    {
        var staff = await db.Users
            .FirstOrDefaultAsync(u => u.UserId == staffUserId, ct);

        if (staff is null || staff.Role != "kitchen_staff")
            throw new Exception("Only kitchen staff can update order status.");

        var order = await db.Orders
            .Include(o => o.OrderItems)
            .FirstOrDefaultAsync(o => o.OrderId == orderId, ct);

        if (order is null)
            throw new Exception("Order not found.");

        var allowed = new[] { "pending", "paid", "preparing", "ready", "completed" };

        if (!allowed.Contains(newStatus))
            throw new Exception("Invalid order status.");
        
        var validTransitions = new Dictionary<string, string[]>
        {
            { "pending",   new[] { "paid" } },
            { "paid",      new[] { "preparing" } },
            { "preparing", new[] { "ready" } },
            { "ready",     new[] { "completed" } }
        };

        if (!validTransitions.TryGetValue(order.Status, out var nextStatuses) ||
            !nextStatuses.Contains(newStatus))
        {
            throw new Exception($"Invalid status transition: {order.Status} → {newStatus}");
        }

        order.Status = newStatus;
        order.UpdatedAt = DateTime.UtcNow;

        await db.SaveChangesAsync(ct);

        return true;
    }
    
    public async Task<Order?> GetBasicOrderAsync(int orderId, CancellationToken ct)
    {
        return await db.Orders
            .Where(o => o.OrderId == orderId)
            .FirstOrDefaultAsync(ct);
    }
    
    public async Task UpdateOrderStatusDirectAsync(Order order)
    {
        db.Orders.Update(order);
        await db.SaveChangesAsync();
    }
    
    public async Task<List<ActiveOrderDto>> GetActiveKitchenOrdersAsync(CancellationToken ct)
    {
        return await db.Orders
            .Where(o => o.Status == "paid" 
                        || o.Status == "preparing" 
                        || o.Status == "ready")
            .Include(o => o.Student)
            .ThenInclude(s => s.User)
            .Include(o => o.OrderItems)
            .ThenInclude(i => i.MenuItem)
            .OrderBy(o => o.CreatedAt)
            .Select(o => new ActiveOrderDto
            {
                OrderId = o.OrderId,
                StudentName = o.Student.User.FirstName + " " + o.Student.User.LastName,
                TotalAmount = o.TotalAmount,
                Status = o.Status,
                CreatedAt = o.CreatedAt,
                Items = o.OrderItems.Select(i => new ActiveOrderItemDto
                {
                    MenuItemId = i.MenuItemId,
                    MenuItemName = i.MenuItem.Name,
                    Quantity = i.Quantity
                }).ToList(),
                Notes = o.Notes 
            })
            .ToListAsync(ct);
    }
    
    public async Task AddOrderNotesAsync(int orderId, string? notes, CancellationToken ct)
    {
        logger.LogInformation("Attempting to update notes for OrderId: {OrderId}", orderId);
        
        var order = await db.Orders.FindAsync(new object[] { orderId }, ct);
        if (order != null)
        {
            order.Notes = notes;
            await db.SaveChangesAsync(ct);
            logger.LogInformation("Order notes updated successfully for OrderId: {OrderId}", orderId);
        }
        else
        {
            logger.LogWarning("Order not found for OrderId: {OrderId}", orderId);
        }
    }




}