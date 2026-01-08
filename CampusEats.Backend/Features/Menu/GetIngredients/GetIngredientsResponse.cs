namespace CampusEats.Features.Menu.GetIngredients;

public record GetIngredientsResponse(
    int IngredientId,
    string Name,
    string? Allergens,
    int CaloriesPerUnit
);