using CampusEats.Features.Orders;
namespace CampusEats.Features.Admin.ManageAdmin;

public static class GetTodayOrdersCountEndpoint
{
    public static async Task<int> Handle(
        IOrderRepository orderRepo,
        CancellationToken ct)
    {
        var count = await orderRepo.GetTodayOrdersCountAsync(ct);
        Console.WriteLine($"🔍 Endpoint returning count: {count}");
        return count;
    }
}