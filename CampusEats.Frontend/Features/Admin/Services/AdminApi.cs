using System.Net.Http.Json;
using System.Text.Json;
using CampusEats.Frontend.Features.Admin.Models;
using CampusEats.Frontend.Features.Admin.Pages;

namespace CampusEats.Frontend.Features.Admin.Services;

public class AdminApi
{
    private readonly HttpClient _http;

    public AdminApi(HttpClient http)
    {
        _http = http;
    }

    // STUDENT REQUESTS
    public async Task<List<StudentRequest>> GetStudentRequests()
    {
        try
        {
            var response = await _http.GetAsync("/admin/students/requests");

            if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                throw new UnauthorizedAccessException("Not authorized to access admin resources");

            var json = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
                return new();

            var result = JsonSerializer.Deserialize<StudentRequestsResponse>(
                json,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true }
            );

            return result?.Items ?? new();
        }
        catch (UnauthorizedAccessException)
        {
            throw; 
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error getting student requests: {ex.Message}");
            return new();
        }
    }
    public async Task<Students.GetStudentsAdminResponse> GetStudentsList(int page, int pageSize)
    {
        var response = await _http.GetAsync($"/admin/students?page={page}&pageSize={pageSize}");
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<Students.GetStudentsAdminResponse>();
    }

    public async Task ApproveStudent(int id)
    {
        var response = await _http.PutAsync($"/admin/students/requests/{id}/approve", null);
        
        if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
            throw new UnauthorizedAccessException();
            
        response.EnsureSuccessStatusCode();
    }

    public async Task RejectStudent(int id)
    {
        var response = await _http.PutAsync($"/admin/students/requests/{id}/reject", null);
        
        if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
            throw new UnauthorizedAccessException();
            
        response.EnsureSuccessStatusCode();
    }

    // ADMINS
    public async Task<List<AdminDto>> GetAdmins()
    {
        try
        {
            var response = await _http.GetAsync("/admin/admins");
            
            if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                throw new UnauthorizedAccessException("Not authorized to access admin resources");

            var json = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
                return new();

            var result = JsonSerializer.Deserialize<AdminListResponse>(
                json,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true }
            );

            return result?.Items ?? new();
        }
        catch (UnauthorizedAccessException)
        {
            throw;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error getting admins: {ex.Message}");
            return new();
        }
    }

    public async Task<bool> CreateAdmin(CreateAdminRequest req)
    {
        var response = await _http.PostAsJsonAsync("/admin/create-admin", req);
        
        if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
            throw new UnauthorizedAccessException();
            
        return response.IsSuccessStatusCode;
    }

    public async Task DeleteAdmin(int id)
    {
        var response = await _http.DeleteAsync($"/admin/{id}");
        
        if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
            throw new UnauthorizedAccessException();
            
        response.EnsureSuccessStatusCode();
    }

    // KITCHEN STAFF
    public async Task<List<KitchenStaffUser>> GetKitchenStaff()
    {
        try
        {
            var response = await _http.GetAsync("/admin/kitchen-staff");

            if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                throw new UnauthorizedAccessException("Not authorized to access admin resources");

            var json = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
                return new();

            var result = JsonSerializer.Deserialize<KitchenStaffResponse>(
                json,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true }
            );

            return result?.Items ?? new();
        }
        catch (UnauthorizedAccessException)
        {
            throw;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error getting kitchen staff: {ex.Message}");
            return new();
        }
    }
    public async Task<int> GetTodayOrdersCount()
    {
        try
        {
            var response = await _http.GetAsync("/admin/orders/today-count");
    
            if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                throw new UnauthorizedAccessException();
        
            response.EnsureSuccessStatusCode();
            
            var count = await response.Content.ReadFromJsonAsync<int>();
            Console.WriteLine($"📊 Today orders count: {count}");
            return count;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"❌ Error getting today orders count: {ex.Message}");
            return 0;
        }
    }

    public async Task<bool> CreateKitchenStaff(CreateKitchenStaffRequest req)
    {
        var response = await _http.PostAsJsonAsync("/admin/kitchen-staff", req);
        
        if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
            throw new UnauthorizedAccessException();
            
        return response.IsSuccessStatusCode;
    }

    public async Task DeleteKitchenStaff(int id)
    {
        var response = await _http.DeleteAsync($"/admin/kitchen-staff/{id}");
        
        if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
            throw new UnauthorizedAccessException();
            
        response.EnsureSuccessStatusCode();
    }
    
    //PAYMENT ANALYTICS OVERVIEW
    public async Task<PaymentAnalyticsOverview> GetPaymentAnalyticsOverview(string period = "all")
    {
        var response = await _http.GetAsync(
            $"/admin/analytics/payments/overview?period={period}"
        );

        if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
            throw new UnauthorizedAccessException();

        response.EnsureSuccessStatusCode();

        return await response.Content.ReadFromJsonAsync<PaymentAnalyticsOverview>()
               ?? throw new Exception("Failed to get analytics overview");
    }


    // Daily Revenue
    public async Task<List<DailyRevenueData>> GetDailyRevenue(DateTime? from = null, DateTime? to = null)
    {
        var url = "/admin/analytics/payments/daily";

        if (from.HasValue || to.HasValue)
        {
            var query = new List<string>();
            
            if (from.HasValue) 
            {
                var fromUtc = from.Value.ToUniversalTime();
                query.Add($"from={fromUtc:yyyy-MM-ddTHH:mm:ssZ}");
            }
        
            if (to.HasValue) 
            {
                var toUtc = to.Value.ToUniversalTime();
                query.Add($"to={toUtc:yyyy-MM-ddTHH:mm:ssZ}");
            }
        
            url += "?" + string.Join("&", query);
        }

        var response = await _http.GetAsync(url);

        if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
            throw new UnauthorizedAccessException();

        response.EnsureSuccessStatusCode();

        return await response.Content.ReadFromJsonAsync<List<DailyRevenueData>>() ?? new();
    }


    // Monthly Revenue
    public async Task<List<MonthlyRevenueData>> GetMonthlyRevenue(int months = 12)
    {
        var response = await _http.GetAsync(
            $"/admin/analytics/payments/monthly?months={months}"
        );

        if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
            throw new UnauthorizedAccessException();

        response.EnsureSuccessStatusCode();

        return await response.Content.ReadFromJsonAsync<List<MonthlyRevenueData>>() ?? new();
    }

    // Transactions
    public async Task<PaymentTransactionsResponse> GetPaymentTransactions(
        int page = 1,
        int pageSize = 20,
        string? status = null)
    {
        var url = $"/admin/analytics/payments/transactions?page={page}&pageSize={pageSize}";

        if (!string.IsNullOrWhiteSpace(status))
            url += $"&status={status}";

        var response = await _http.GetAsync(url);

        if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
            throw new UnauthorizedAccessException();

        response.EnsureSuccessStatusCode();

        return await response.Content.ReadFromJsonAsync<PaymentTransactionsResponse>()
               ?? throw new Exception("Failed to get transactions");
    }
    
public record PaymentAnalyticsOverview(
    decimal TotalRevenue,
    decimal RevenueToday,
    decimal RevenueThisMonth,
    int TotalOrders,
    decimal AverageOrderValue,
    string Period
);

public record DailyRevenueData(
    DateTime Date,
    decimal Revenue,
    int Orders
);

public record MonthlyRevenueData(
    int Year,
    int Month,
    decimal Revenue,
    int Orders
);

public record PaymentTransaction(
    int OrderId,
    decimal Amount,
    string Status,
    string StudentName,
    string StudentEmail,
    DateTime CreatedAt,
    string? StripePaymentIntentId
);

public record PaymentTransactionsResponse(
    List<PaymentTransaction> Items,
    int TotalCount,
    int Page,
    int PageSize,
    int TotalPages,
    bool HasNextPage,
    bool HasPreviousPage
);
}