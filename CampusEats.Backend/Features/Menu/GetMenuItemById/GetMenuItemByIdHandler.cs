using CampusEats.Features.Menu.Interfaces;
using CampusEats.Shared.Exceptions;
using MediatR;

namespace CampusEats.Features.Menu.GetMenuItemById
{
    public class GetMenuItemByIdHandler(
        IMenuRepository menuRepo,
        IFavoritesRepository favoritesRepo,
        IHttpContextAccessor http
    ) : IRequestHandler<GetMenuItemByIdQuery, MenuItemDto>
    {
        public async Task<MenuItemDto> Handle(GetMenuItemByIdQuery request, CancellationToken ct)
        {
            var items = await menuRepo.GetMenuItemsByIdsAsync(
                new List<int> { request.MenuItemId }, ct);

            if (!items.TryGetValue(request.MenuItemId, out var item))
                throw new NotFoundException($"Menu item {request.MenuItemId} not found.");
            
            var studentIdClaim = http.HttpContext?.User.FindFirst("nameid")?.Value;
            int.TryParse(studentIdClaim, out var studentId);
            
            bool isFavorite = await favoritesRepo.ExistsAsync(studentId, item.MenuItemId);

            var labels = item.MenuItemLabels
                .Select(l => l.LabelId.ToString())
                .ToList();

            var ingredients = item.MenuItemIngredients
                .Select(i => new MenuIngredientDetailDto(
                    i.IngredientId,
                    i.Ingredient.Name,
                    i.Quantity,
                    i.Ingredient.Allergens,
                    i.Ingredient.CaloriesPerUnit
                ))
                .ToList();

            return new MenuItemDto(
                item.MenuItemId,
                item.Name,
                item.Description,
                item.PictureLink,
                item.Price,
                item.Calories,
                item.IsAvailable,
                item.CurrentStock,
                labels,
                ingredients,
                isFavorite
            );
        }
    }
}