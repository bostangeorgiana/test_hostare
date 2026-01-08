using MediatR;
using CampusEats.Features.Orders;

namespace CampusEats.Features.Orders.GetActiveOrders;

public class GetActiveOrdersHandler(IOrderRepository orderRepo)
    : IRequestHandler<GetActiveOrdersCommand, List<ActiveOrderDto>>
{
    public async Task<List<ActiveOrderDto>> Handle(GetActiveOrdersCommand request, CancellationToken ct)
    {
        return await orderRepo.GetActiveKitchenOrdersAsync(ct);
    }
}