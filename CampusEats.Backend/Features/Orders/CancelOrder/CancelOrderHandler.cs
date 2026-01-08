using MediatR;

namespace CampusEats.Features.Orders.CancelOrder;

public class CancelOrderHandler(IOrderRepository repo) : IRequestHandler<CancelOrderCommand, bool>
{
    public async Task<bool> Handle(CancelOrderCommand request, CancellationToken ct)
    {
        return await repo.CancelOrderAsync(request.OrderId, request.StudentId, ct);
    }
}