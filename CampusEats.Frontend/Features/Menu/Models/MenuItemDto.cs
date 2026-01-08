namespace CampusEats.Frontend.Features.Menu.Models;

public class MenuItemDto
{
    public int MenuItemId { get; set; }
    public string Name { get; set; } = "";
    public string Description { get; set; } = "";
    public decimal Price { get; set; }
    public int Calories { get; set; }
    public bool IsAvailable { get; set; }
    public int CurrentStock { get; set; }
    
    public List<string> Labels { get; set; } = new();
    
    public List<MenuIngredientDetailDto> Ingredients { get; set; } = new();
    
    public bool IsFavorite { get; set; } = false;
    
    public string? PictureLink { get; set; }
}