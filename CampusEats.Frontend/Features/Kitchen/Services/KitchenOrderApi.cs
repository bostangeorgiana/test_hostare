using System.Net.Http.Json;
using CampusEats.Frontend.Features.Kitchen.Models;
using CampusEats.Frontend.Features.Student.Order.Models;

namespace CampusEats.Frontend.Features.Kitchen.Services;

public class KitchenOrderApi(HttpClient http)
{
    public async Task<List<OrderSummaryDto>> GetActiveOrders()
    {
        return await http.GetFromJsonAsync<List<OrderSummaryDto>>("/api/orders/kitchen/active")
               ?? new List<OrderSummaryDto>();
    }

    public async Task<bool> UpdateStatus(int orderId, string newStatus, int staffId)
    {
        var response = await http.PatchAsJsonAsync<object>(
            $"/api/orders/{orderId}/status?newStatus={newStatus}&staffUserId={staffId}",
            new { } 
        );

        return response.IsSuccessStatusCode;
    }

    public async Task<OrderDetailsDto> GetOrderDetails(int orderId)
    {
        return await http.GetFromJsonAsync<OrderDetailsDto>($"/api/orders/{orderId}")
               ?? new OrderDetailsDto();
    }
}