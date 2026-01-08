using MediatR;

namespace CampusEats.Features.Orders.GetOrderHistory;

public record GetOrderHistoryCommand(int StudentId)
    : IRequest<List<OrderSummaryDto>>;