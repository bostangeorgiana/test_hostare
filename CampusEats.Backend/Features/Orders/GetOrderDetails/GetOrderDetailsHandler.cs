using MediatR;
using CampusEats.Persistence.Repositories;

namespace CampusEats.Features.Orders.GetOrderDetails;

public class GetOrderDetailsHandler(IOrderRepository repository)
    : IRequestHandler<GetOrderDetailsCommand, OrderDetailsDto?>
{
    public Task<OrderDetailsDto?> Handle(
        GetOrderDetailsCommand request,
        CancellationToken cancellationToken)
    {
        return repository.GetOrderDetailsAsync(request.OrderId, cancellationToken);
    }
}