// ...existing code...
namespace CampusEats.Features.Recommendations.Models;

public class RecommendedMenuItem
{
    public int MenuItemId { get; set; }
    public string Name { get; set; } = string.Empty;
    public decimal Score { get; set; }
    public string? PictureLink { get; set; }
}
