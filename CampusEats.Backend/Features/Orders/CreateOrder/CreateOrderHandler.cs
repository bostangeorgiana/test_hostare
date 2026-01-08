using MediatR;
using CampusEats.Exceptions;
using CampusEats.Features.Menu.Interfaces;

namespace CampusEats.Features.Orders.CreateOrder;

public class CreateOrderHandler(
    IOrderRepository orderRepo,
    IMenuRepository menuRepo,
    IUnitOfWork uow,
    ILogger<CreateOrderHandler> logger)
    : IRequestHandler<CreateOrderCommand, int>
{
    public async Task<int> Handle(CreateOrderCommand request, CancellationToken ct)
    {
        try
        {
            if (request.Items == null || !request.Items.Any())
                throw new DomainException("Cannot create an order with no items.");

            var menuItemIds = request.Items
                .Select(i => i.MenuItemId)
                .Distinct()
                .ToList();

            var menuItems = await menuRepo.GetMenuItemsByIdsAsync(menuItemIds, ct);

            decimal total = 0m;
            var orderItems = new List<(int MenuItemId, int Quantity, decimal Price)>();
            
            foreach (var item in request.Items)
            {
                if (item.Quantity <= 0)
                    throw new DomainException($"Quantity must be greater than 0 for item {item.MenuItemId}.");

                if (!menuItems.TryGetValue(item.MenuItemId, out var menuItem))
                    throw new DomainException($"Menu item {item.MenuItemId} not found.");

                if (menuItem.CurrentStock < item.Quantity)
                    throw new DomainException($"Insufficient stock for '{menuItem.Name}'.");

                total += menuItem.Price * item.Quantity;
                orderItems.Add((item.MenuItemId, item.Quantity, menuItem.Price));
            }
            
            var orderId = await orderRepo.CreateOrderAsync(
                request.StudentId,
                total,
                status: "pending",
                notes: request.Notes, 
                ct);

            await orderRepo.AddOrderItemsAsync(orderId, orderItems, ct);
            

            await uow.SaveChangesAsync();

            return orderId;
        }
        catch (DomainException ex)
        {
            logger.LogError(ex, "Error creating order: {Message}", ex.Message);
            throw;
        }
    }
}