using System.Net.Http.Json;
using CampusEats.Frontend.Features.Recommendations.Models;
using CampusEats.Frontend.Features.Menu.Services;

namespace CampusEats.Frontend.Features.Recommendations;

public class RecommendationsApi(HttpClient http, MenuApi menuApi)
{
    public async Task<List<FullRecommendedItemDto>> GetCartRecommendationsAsync(List<int> itemIds, int limit = 3)
    {
        if (itemIds.Count == 0)
            return new List<FullRecommendedItemDto>();

        string ids = string.Join(",", itemIds);
        string url = $"/recommendations/cart?itemIds={ids}&limit={limit}";

        var simple = await http.GetFromJsonAsync<List<RecommendedItemDto>>(url);
        if (simple == null || simple.Count == 0)
            return new List<FullRecommendedItemDto>();

        var full = new List<FullRecommendedItemDto>();
        foreach (var item in simple)
        {
            var menuItem = await menuApi.GetMenuItemAsync(item.MenuItemId);
            if (menuItem == null)
                continue;

            full.Add(new FullRecommendedItemDto
            {
                MenuItemId = item.MenuItemId,
                Name = menuItem.Name,
                Price = menuItem.Price,
                Calories = menuItem.Calories,
                Labels = menuItem.Labels,
                CurrentStock = menuItem.CurrentStock,
                IsAvailable = menuItem.IsAvailable,
                PictureLink = string.IsNullOrWhiteSpace(menuItem.PictureLink) ? item.PictureLink : menuItem.PictureLink,
                Score = item.Score
            });
        }

        return full;
    }
}
