using System.Net.Http.Json;
using CampusEats.Frontend.Features.Student.Order.Models;
using CampusEats.Frontend.Models;

namespace CampusEats.Frontend.Services;

public class OrderApi(HttpClient http)
{
    public async Task<int?> CreateOrder(CreateOrderRequest req)
    {
        var response = await http.PostAsJsonAsync("/api/orders", req);

        if (!response.IsSuccessStatusCode)
            return null;

        return await response.Content.ReadFromJsonAsync<int>();
    }

    public Task<List<OrderSummary>?> GetHistory(int studentId)
        => http.GetFromJsonAsync<List<OrderSummary>>($"/api/orders/history/{studentId}");

    public Task<OrderDetailsDto?> GetDetails(int orderId)
        => http.GetFromJsonAsync<OrderDetailsDto>($"/api/orders/{orderId}");


    public Task CancelOrder(int orderId, int studentId)
        => http.PatchAsync($"/api/orders/{orderId}/cancel?studentId={studentId}", null);
    
    public Task<LoyaltyResponse?> GetLoyalty(int studentId)
        => http.GetFromJsonAsync<LoyaltyResponse>($"/api/loyalty/{studentId}");
}