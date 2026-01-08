using CampusEats.Middleware;
using CampusEats.Features.HealthChecks;
using CampusEats.Features.Auth;
using CampusEats.Features.Admin;
using CampusEats.Features.Menu;
using CampusEats.Features.Orders;
using CampusEats.Features.Payment;
using CampusEats.Features.Loyalty;
using CampusEats.Features.Users;
using CampusEats.Features.Recommendations.GetItemRecommendations;
using CampusEats.Features.Recommendations.GetCartRecommendations;

namespace CampusEats.Configuration;

public static class AppExtensions
{
    public static WebApplication UseAppPipeline(this WebApplication app)
    {
        app.UseMiddleware<ExceptionHandlingMiddleware>();
        app.UseMiddleware<LoggingMiddleware>();

        app.UseHttpsRedirection();
        app.UseCors("DefaultCorsPolicy");
        app.UseAuthentication();
        app.UseAuthorization();

        return app;
    }
    
    public static WebApplication MapAppEndpoints(this WebApplication app)
    {
        app.MapGet("/health", async (GetDbHealthHandler handler) =>
            await handler.Handle());
        
        app.MapAuthEndpoints();
        app.MapAdminEndpoints();
        app.MapMenuEndpoints();
        app.MapOrderEndpoints();
        app.MapPaymentEndpoints();
        app.MapLoyaltyEndpoints();
        app.MapUserEndpoints();
        app.MapGetItemRecommendations();
        app.MapGetCartRecommendations();
        
        return app;
    }
    
    public static WebApplication UseDevelopmentTools(this WebApplication app)
    {
        if (app.Environment.IsDevelopment())
        {
            app.MapOpenApi();
        }

        return app;
    }
}