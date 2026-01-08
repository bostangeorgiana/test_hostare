using CampusEats.Features.Menu.CreateMenuItem;
using CampusEats.Features.Menu.GetMenuLabels;

namespace CampusEats.Features.Menu.Interfaces;

public interface IMenuRepository
{
    Task<int> CreateMenuItemAsync(
        string name,
        string description,
        decimal price,
        int calories,
        int stock,
        string? pictureLink);

    Task AssignLabelsAsync(int menuItemId, List<int> labelIds);

    Task AssignIngredientsAsync(int menuItemId, List<MenuIngredientDto> ingredients);

    Task<List<MenuItemDto>> GetMenuListAsync(
        List<int>? labelIds,
        AvailabilityFilter availability,
        bool onlyFavorites,
        decimal? minPrice,
        decimal? maxPrice);

    Task UpdateStockAsync(int menuItemId, int stock);

    Task<Dictionary<int, Persistence.Entities.MenuItem>> GetMenuItemsByIdsAsync(
        List<int> ids,
        CancellationToken ct);

    Task DecreaseStockAsync(int menuItemId, int quantity, CancellationToken ct);

    Task DeleteMenuItemAsync(int menuItemId);

    Task<List<GetMenuLabelsResponse>> GetMenuLabelsAsync(CancellationToken ct);
}