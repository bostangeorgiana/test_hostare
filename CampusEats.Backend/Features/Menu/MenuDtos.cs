namespace CampusEats.Features.Menu;

public record MenuItemDto(
    int MenuItemId,
    string Name,
    string Description,
    string? PictureLink,
    decimal Price,
    int Calories,
    bool IsAvailable,
    int CurrentStock,
    List<string> Labels,
    List<MenuIngredientDetailDto> Ingredients,
    bool IsFavorite
);

public record MenuIngredientDetailDto(
    int IngredientId,
    string Name,
    decimal Quantity,
    string? Allergens,
    int CaloriesPerUnit
);


