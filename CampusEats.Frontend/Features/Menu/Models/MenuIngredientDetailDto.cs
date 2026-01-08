namespace CampusEats.Frontend.Features.Menu.Models;

public class MenuIngredientDetailDto
{
    public int IngredientId { get; set; }
    public string Name { get; set; } = "";
    public decimal Quantity { get; set; }
    public string? Allergens { get; set; }
    public int CaloriesPerUnit { get; set; }
}