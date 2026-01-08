namespace CampusEats.Frontend.Features.Recommendations.Models;

public class RecommendedItemDto
{
    public int MenuItemId { get; set; }
    public string Name { get; set; } = "";
    public decimal Score { get; set; }
    public string? PictureLink { get; set; }
}
