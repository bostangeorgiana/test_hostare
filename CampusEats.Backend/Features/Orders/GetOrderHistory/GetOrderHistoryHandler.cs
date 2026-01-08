using MediatR;
using CampusEats.Persistence.Repositories;

namespace CampusEats.Features.Orders.GetOrderHistory;

public class GetOrderHistoryHandler(IOrderRepository repository)
    : IRequestHandler<GetOrderHistoryCommand, List<OrderSummaryDto>>
{
    public Task<List<OrderSummaryDto>> Handle(
        GetOrderHistoryCommand request,
        CancellationToken cancellationToken)
    {
        return repository.GetOrderHistoryAsync(request.StudentId, cancellationToken);
    }
}