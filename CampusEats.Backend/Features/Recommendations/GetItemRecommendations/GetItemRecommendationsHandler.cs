using MediatR;
using CampusEats.Shared;
using CampusEats.Persistence.Repositories;
using CampusEats.Features.Recommendations.Models;

namespace CampusEats.Features.Recommendations.GetItemRecommendations;

public class GetItemRecommendationsHandler(
    IRecommendationRepository recommendations)
    : IRequestHandler<GetItemRecommendationsQuery, Result<List<RecommendedMenuItem>>>
{
    public async Task<Result<List<RecommendedMenuItem>>> Handle(GetItemRecommendationsQuery request, CancellationToken ct)
    {
        if (request.MenuItemId <= 0)
            return Result<List<RecommendedMenuItem>>.Failure("Invalid menuItemId");

        var list = await recommendations.GetTopCoPurchasedItemsAsync(request.MenuItemId, request.Limit, ct);
        return Result<List<RecommendedMenuItem>>.Success(list.ToList());
    }
}
