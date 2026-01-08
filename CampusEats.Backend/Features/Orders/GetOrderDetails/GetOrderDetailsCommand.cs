using MediatR;

namespace CampusEats.Features.Orders.GetOrderDetails;

public record GetOrderDetailsCommand(int OrderId)
    : IRequest<OrderDetailsDto?>;