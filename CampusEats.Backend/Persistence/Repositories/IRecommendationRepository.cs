
using CampusEats.Features.Recommendations.Models;

namespace CampusEats.Persistence.Repositories;

public interface IRecommendationRepository
{
    Task<IReadOnlyList<RecommendedMenuItem>> GetTopCoPurchasedItemsAsync(
        int baseMenuItemId,
        int limit = 3,
        CancellationToken cancellationToken = default);
}

