using System.ComponentModel.DataAnnotations.Schema;

namespace CampusEats.Persistence.Entities;

[Table("recommendations")]
public class Recommendation
{
    [Column("base_item_id")]
    public int BaseItemId { get; set; }

    [ForeignKey(nameof(BaseItemId))]
    public MenuItem BaseItem { get; set; } = null!;

    [Column("recommended_item_id")]
    public int RecommendedItemId { get; set; }

    [ForeignKey(nameof(RecommendedItemId))]
    public MenuItem RecommendedItem { get; set; } = null!;

    [Column("score")]
    public decimal Score { get; set; }
}