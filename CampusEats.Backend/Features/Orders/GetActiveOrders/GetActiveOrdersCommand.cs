using MediatR;

namespace CampusEats.Features.Orders.GetActiveOrders;

public record GetActiveOrdersCommand() : IRequest<List<ActiveOrderDto>>;