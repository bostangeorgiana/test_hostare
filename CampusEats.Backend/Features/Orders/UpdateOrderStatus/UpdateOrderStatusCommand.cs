using MediatR;

namespace CampusEats.Features.Orders.UpdateOrderStatus;

public record UpdateOrderStatusCommand(
    int OrderId,
    string NewStatus,
    int StaffUserId
) : IRequest<bool>;