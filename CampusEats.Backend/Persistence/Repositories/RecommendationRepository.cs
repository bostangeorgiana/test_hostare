using CampusEats.Features.Recommendations.Models;
using CampusEats.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace CampusEats.Persistence.Repositories;

public class RecommendationRepository(
    CampusEatsDbContext dbContext)
    : IRecommendationRepository
{
    public async Task<IReadOnlyList<RecommendedMenuItem>> GetTopCoPurchasedItemsAsync(
        int baseMenuItemId,
        int limit = 3,
        CancellationToken cancellationToken = default)
    {
        var finalQuery = from oi in dbContext.OrderItems
                         join oi2 in dbContext.OrderItems on oi.OrderId equals oi2.OrderId
                         join mi in dbContext.MenuItems on oi2.MenuItemId equals mi.MenuItemId
                         where oi.MenuItemId == baseMenuItemId
                               && oi2.MenuItemId != baseMenuItemId
                               && mi.IsAvailable
                               && mi.CurrentStock > 0
                         group oi2 by new { oi2.MenuItemId, mi.Name, mi.PictureLink } into g
                         select new RecommendedMenuItem
                         {
                             MenuItemId = g.Key.MenuItemId,
                             Name = g.Key.Name,
                             PictureLink = g.Key.PictureLink,
                             Score = g.Sum(x => x.Quantity)
                         };

        var finalResults = await finalQuery
            .OrderByDescending(r => r.Score)
            .Take(limit)
            .ToListAsync(cancellationToken);

        return finalResults;
    }
}
