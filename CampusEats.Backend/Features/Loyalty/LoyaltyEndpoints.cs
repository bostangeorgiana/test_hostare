using CampusEats.Features.Loyalty.AddLoyaltyPoints;
using CampusEats.Features.Loyalty.GetLoyaltyPoints;
using Microsoft.AspNetCore.Mvc;

namespace CampusEats.Features.Loyalty;

public static class LoyaltyEndpoints
{
    public static IEndpointRouteBuilder MapLoyaltyEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/loyalty")
            .WithTags("Loyalty");
        
        group.MapPost("/add", async (
            AddLoyaltyPointsRequest request,
            AddLoyaltyPointsHandler handler) =>
        {
            var result = await handler.HandleAsync(request);
            return Results.Ok(result);
        });

        group.MapGet("/{studentId:int}", async (
            int studentId,
            [FromServices] GetLoyaltyPointsHandler handler) =>
        {
            try
            {
                var response = await handler.HandleAsync(new GetLoyaltyPointsQuery(studentId));
                return Results.Ok(response);
            }
            catch
            {
                return Results.NotFound();
            }
        });


        return app;
    }
}