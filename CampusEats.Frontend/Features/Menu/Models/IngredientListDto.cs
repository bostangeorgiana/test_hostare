namespace CampusEats.Frontend.Features.Menu.Models;

public class IngredientListDto
{
    public int IngredientId { get; set; }
    public string Name { get; set; } = "";
    public string? Allergens { get; set; }
    public int CaloriesPerUnit { get; set; }
}