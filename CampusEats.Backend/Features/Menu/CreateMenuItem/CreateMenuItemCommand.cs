using MediatR;

namespace CampusEats.Features.Menu.CreateMenuItem;

public record CreateMenuItemCommand(
    string Name,
    string Description,
    decimal Price,
    int Calories,
    int Stock,
    List<int> LabelIds,
    List<MenuIngredientDto> Ingredients,
    string? PictureLink
) : IRequest<int>;


public record MenuIngredientDto(
    int IngredientId,
    decimal Quantity,
    string Name,
    string? Allergens,
    int CaloriesPerUnit
);
