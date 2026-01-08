using CampusEats.Features.Menu.Interfaces;
using MediatR;

namespace CampusEats.Features.Menu.Favorites;

public class GetFavoriteMenuItemsByStudentHandler 
    : IRequestHandler<GetFavoriteMenuItemsByStudentCommand, List<MenuItemDto>>
{
    private readonly IFavoritesRepository _favoritesRepository;
    private readonly IMenuRepository _menuRepository;

    public GetFavoriteMenuItemsByStudentHandler(
        IFavoritesRepository favoritesRepository,
        IMenuRepository menuRepository)
    {
        _favoritesRepository = favoritesRepository;
        _menuRepository = menuRepository;
    }

    public async Task<List<MenuItemDto>> Handle(
        GetFavoriteMenuItemsByStudentCommand request, 
        CancellationToken cancellationToken)
    {
        // 1. Get favorite menu item IDs
        var favoriteIds = await _favoritesRepository.GetFavoriteMenuItemIdsAsync(request.StudentId);

        if (!favoriteIds.Any())
            return new List<MenuItemDto>();

        var itemsDict = await _menuRepository.GetMenuItemsByIdsAsync(favoriteIds, cancellationToken);

        // 3. Convert to DTOs
        var items = itemsDict.Values.Select(menuItem =>
            new MenuItemDto(
                MenuItemId: menuItem.MenuItemId,
                Name: menuItem.Name,
                Description: menuItem.Description ?? "",
                PictureLink: menuItem.PictureLink,
                Price: menuItem.Price,
                Calories: menuItem.Calories,
                IsAvailable: menuItem.IsAvailable,
                CurrentStock: menuItem.CurrentStock,
                Labels: menuItem.MenuItemLabels
                    .Select(l => l.Label.Name) // Label.Name
                    .ToList(),
                Ingredients: menuItem.MenuItemIngredients
                    .Select(i => new MenuIngredientDetailDto(
                        IngredientId: i.Ingredient.IngredientId,
                        Name: i.Ingredient.Name,
                        Quantity: i.Quantity,
                        Allergens: i.Ingredient.Allergens,
                        CaloriesPerUnit: i.Ingredient.CaloriesPerUnit
                    ))
                    .ToList(),
                IsFavorite: true
            )
        ).ToList();

        return items;

    }
}