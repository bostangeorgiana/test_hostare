using MediatR;

namespace CampusEats.Features.Orders.UpdateOrderStatus;

public class UpdateOrderStatusHandler(IOrderRepository repo) : IRequestHandler<UpdateOrderStatusCommand, bool>
{
    public async Task<bool> Handle(UpdateOrderStatusCommand request, CancellationToken ct)
    {
        return await repo.UpdateOrderStatusAsync(
            request.OrderId,
            request.NewStatus,
            request.StaffUserId,
            ct
        );
    }
}