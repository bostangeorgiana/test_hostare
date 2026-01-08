using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace CampusEats.Features.Recommendations.GetItemRecommendations;

public static class GetItemRecommendationsEndpoint
{
    public static void MapGetItemRecommendations(this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("/recommendations");

        group.MapGet("/for-item/{menuItemId}", async (
            int menuItemId,
            [FromQuery] int? limit,
            IMediator mediator) =>
        {
            if (menuItemId <= 0)
                return Results.BadRequest("menuItemId must be greater than 0");

            var query = new GetItemRecommendationsQuery(menuItemId, limit ?? 3);
            var result = await mediator.Send(query);

            if (!result.IsSuccess)
                return Results.BadRequest(result.Error);
            
            return Results.Ok(result.Value);
        });
    }
}
