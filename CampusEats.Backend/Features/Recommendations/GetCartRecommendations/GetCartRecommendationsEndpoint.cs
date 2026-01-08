using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace CampusEats.Features.Recommendations.GetCartRecommendations;

public static class GetCartRecommendationsEndpoint
{
    public static void MapGetCartRecommendations(this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("/recommendations");

        group.MapGet("/cart", async (
            [FromQuery] string itemIds,
            [FromQuery] int? limit,
            IMediator mediator) =>
        {
            if (string.IsNullOrWhiteSpace(itemIds))
                return Results.BadRequest("itemIds query parameter is required. Example: ?itemIds=1,4,6");

            var ids = itemIds.Split(',', StringSplitOptions.RemoveEmptyEntries)
                .Select(s => int.TryParse(s.Trim(), out var v) ? v : 0)
                .Where(i => i > 0)
                .ToList();

            if (ids.Count == 0)
                return Results.BadRequest("No valid itemIds provided.");

            var query = new GetCartRecommendationsQuery(ids, limit ?? 3);
            var result = await mediator.Send(query);

            if (!result.IsSuccess)
                return Results.BadRequest(result.Error);

            return Results.Ok(result.Value);
        });
    }
}
