using CampusEats.Features.Orders.GetActiveOrders;
using CampusEats.Persistence.Entities;

namespace CampusEats.Features.Orders;

public interface IOrderRepository
{
    Task<int> CreateOrderAsync(int studentId, decimal totalAmount, string status, string? notes, CancellationToken ct);
    Task AddOrderItemsAsync(int orderId, List<(int MenuItemId, int Quantity, decimal Price)> items, CancellationToken ct);
    Task AddOrderNotesAsync(int orderId, string? notes, CancellationToken ct); 
    Task<List<OrderSummaryDto>> GetOrderHistoryAsync(int studentId, CancellationToken ct);
    Task<OrderDetailsDto?> GetOrderDetailsAsync(int orderId, CancellationToken ct);
    
    Task<bool> CancelOrderAsync(int orderId, int studentId, CancellationToken ct);
    
    Task<bool> UpdateOrderStatusAsync(int orderId, string newStatus, int staffUserId, CancellationToken ct);
    
    Task<Order?> GetBasicOrderAsync(int orderId, CancellationToken ct);
    
    Task UpdateOrderStatusDirectAsync(Order order);
    
   Task<List<ActiveOrderDto>> GetActiveKitchenOrdersAsync(CancellationToken ct);
   
   Task<int> GetTodayOrdersCountAsync(CancellationToken ct = default);



}