namespace CampusEats.Frontend.Features.Menu.Models;

public class CreateMenuItemDto
{
    public string Name { get; set; } = "";
    public string Description { get; set; } = "";
    public decimal? Price { get; set; }
    public int? Calories { get; set; }
    public int? Stock { get; set; }
    public List<int> LabelIds { get; set; } = new();
    public List<MenuIngredientDto> Ingredients { get; set; } = new();
    public string? PictureLink { get; set; }
}