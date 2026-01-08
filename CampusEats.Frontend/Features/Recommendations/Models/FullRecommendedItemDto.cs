namespace CampusEats.Frontend.Features.Recommendations.Models;

public class FullRecommendedItemDto
{
    public int MenuItemId { get; set; }
    public string Name { get; set; } = "";
    public decimal Price { get; set; }
    public int Calories { get; set; }
    public List<string> Labels { get; set; } = new();
    public int CurrentStock { get; set; }
    public bool IsAvailable { get; set; }
    public decimal Score { get; set; }
    public string? PictureLink { get; set; }
}
