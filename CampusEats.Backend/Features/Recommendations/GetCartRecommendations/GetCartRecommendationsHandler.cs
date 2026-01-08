using CampusEats.Shared;
using CampusEats.Features.Recommendations.Models;
using CampusEats.Persistence.Repositories;
using MediatR;

namespace CampusEats.Features.Recommendations.GetCartRecommendations;

public class GetCartRecommendationsHandler(
    IRecommendationRepository recommendations)
    : IRequestHandler<GetCartRecommendationsQuery, Result<List<RecommendedMenuItem>>>
{
    public async Task<Result<List<RecommendedMenuItem>>> Handle(GetCartRecommendationsQuery request, CancellationToken ct)
    {
        if (request.ItemIds.Count == 0)
            return Result<List<RecommendedMenuItem>>.Failure("ItemIds must be provided.");

        var combined = new Dictionary<int, RecommendedMenuItem>();

        foreach (var itemId in request.ItemIds)
        {
            var recs = await recommendations.GetTopCoPurchasedItemsAsync(itemId, 3, ct);
            foreach (var r in recs)
            {
                if (request.ItemIds.Contains(r.MenuItemId))
                    continue; 

                if (combined.TryGetValue(r.MenuItemId, out var existing))
                {
                    existing.Score += r.Score;
                    if (string.IsNullOrWhiteSpace(existing.PictureLink) && !string.IsNullOrWhiteSpace(r.PictureLink))
                    {
                        existing.PictureLink = r.PictureLink;
                    }
                }
                else
                {
                    combined[r.MenuItemId] = new RecommendedMenuItem
                    {
                        MenuItemId = r.MenuItemId,
                        Name = r.Name,
                        PictureLink = r.PictureLink,
                        Score = r.Score
                    };
                }
            }
        }
        
        var final = combined.Values
            .OrderByDescending(x => x.Score)
            .Take(request.Limit)
            .ToList();

        return Result<List<RecommendedMenuItem>>.Success(final);
    }
}
