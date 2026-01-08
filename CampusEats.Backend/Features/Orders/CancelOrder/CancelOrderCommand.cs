using MediatR;

namespace CampusEats.Features.Orders.CancelOrder;

public record CancelOrderCommand(int OrderId, int StudentId) : IRequest<bool>;