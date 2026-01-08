using MediatR;

namespace CampusEats.Features.Orders.CreateOrder;

public record CreateOrderCommand(
    int StudentId,
    List<OrderItemDto> Items,
    string? Notes 
) : IRequest<int>;

public record OrderItemDto(
    int MenuItemId,
    int Quantity
);