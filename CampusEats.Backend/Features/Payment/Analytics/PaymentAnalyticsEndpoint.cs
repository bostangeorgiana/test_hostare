using CampusEats.Persistence.Context;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CampusEats.Features.Payment.Analytics;

public static class PaymentAnalyticsEndpoint
{
    public static IEndpointRouteBuilder MapPaymentAnalyticsEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/admin/analytics/payments")
            .RequireAuthorization(policy => policy.RequireRole("admin"));

        group.MapGet("/overview", GetOverview);
        group.MapGet("/daily", GetDailyRevenue);
        group.MapGet("/monthly", GetMonthlyRevenue);
        group.MapGet("/transactions", GetTransactions);

        return app;
    }


    private static async Task<IResult> GetOverview(
        [FromServices] CampusEatsDbContext db,
        string? period = "all")
    {
        var now = DateTime.UtcNow;

        DateTime? startDate = period switch
        {
            "today"  => DateTime.SpecifyKind(now.Date, DateTimeKind.Utc),
            "7days"  => now.AddDays(-7),
            "30days" => now.AddDays(-30),
            _        => null
        };

        var query = db.Orders.Where(o => o.Status == "paid");

        if (startDate.HasValue)
            query = query.Where(o => o.CreatedAt >= startDate.Value);

        var totalRevenue = await query.SumAsync(o =>
            o.FinalAmountPaid > 0 ? o.FinalAmountPaid : o.TotalAmount);

        var totalOrders = await query.CountAsync();

        var averageOrderValue = totalOrders > 0
            ? totalRevenue / totalOrders
            : 0;

        var todayStart = DateTime.SpecifyKind(now.Date, DateTimeKind.Utc);
        
        var revenueToday = await db.Orders
            .Where(o => o.Status == "paid" && o.CreatedAt >= todayStart)
            .SumAsync(o => o.FinalAmountPaid > 0
                ? o.FinalAmountPaid
                : o.TotalAmount);

        var startOfMonth = DateTime.SpecifyKind(
            new DateTime(now.Year, now.Month, 1), 
            DateTimeKind.Utc
        );

        var revenueThisMonth = await db.Orders
            .Where(o => o.Status == "paid" && o.CreatedAt >= startOfMonth)
            .SumAsync(o => o.FinalAmountPaid > 0
                ? o.FinalAmountPaid
                : o.TotalAmount);

        return Results.Ok(new
        {
            totalRevenue,
            revenueToday,
            revenueThisMonth,
            totalOrders,
            averageOrderValue,
            period
        });
    }
    
    private static async Task<IResult> GetDailyRevenue(
        [FromServices] CampusEatsDbContext db,
        DateTime? from = null,
        DateTime? to = null)
    {
        var now = DateTime.UtcNow;
        
        var startDate = (from ?? now.AddDays(-30)).ToUniversalTime();
        var endDate = (to ?? now).ToUniversalTime();

        var dailyData = await db.Orders
            .Where(o => o.Status == "paid"
                        && o.CreatedAt >= startDate
                        && o.CreatedAt <= endDate)
            .GroupBy(o => o.CreatedAt.Date)
            .Select(g => new
            {
                date = g.Key,
                revenue = g.Sum(o => o.FinalAmountPaid > 0
                    ? o.FinalAmountPaid
                    : o.TotalAmount),
                orders = g.Count()
            })
            .OrderBy(x => x.date)
            .ToListAsync();

        return Results.Ok(dailyData);
    }
    
    private static async Task<IResult> GetMonthlyRevenue(
        [FromServices] CampusEatsDbContext db,
        int months = 12)
    {
        var startDate = DateTime.UtcNow.AddMonths(-months);

        var monthlyData = await db.Orders
            .Where(o => o.Status == "paid" && o.CreatedAt >= startDate)
            .GroupBy(o => new { o.CreatedAt.Year, o.CreatedAt.Month })
            .Select(g => new
            {
                year = g.Key.Year,
                month = g.Key.Month,
                revenue = g.Sum(o => o.FinalAmountPaid > 0
                    ? o.FinalAmountPaid
                    : o.TotalAmount),
                orders = g.Count()
            })
            .OrderBy(x => x.year)
            .ThenBy(x => x.month)
            .ToListAsync();

        return Results.Ok(monthlyData);
    }


    private static async Task<IResult> GetTransactions(
        [FromServices] CampusEatsDbContext db,
        int page = 1,
        int pageSize = 20,
        string? status = null)
    {
        var query = db.Orders.AsQueryable();

        if (!string.IsNullOrWhiteSpace(status))
            query = query.Where(o => o.Status == status);

        var totalCount = await query.CountAsync();
        var totalPages = (int)Math.Ceiling(totalCount / (double)pageSize);

        var items = await query
            .OrderByDescending(o => o.CreatedAt)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(o => new
            {
                orderId = o.OrderId,
                amount = o.FinalAmountPaid > 0
                    ? o.FinalAmountPaid
                    : o.TotalAmount,
                status = o.Status,
                studentName = o.Student.User.FirstName + " " + o.Student.User.LastName,
                studentEmail = o.Student.User.Email,
                createdAt = o.CreatedAt
            })
            .ToListAsync();

        return Results.Ok(new
        {
            items,
            totalCount,
            page,
            pageSize,
            totalPages,
            hasNextPage = page < totalPages,
            hasPreviousPage = page > 1
        });
    }
}