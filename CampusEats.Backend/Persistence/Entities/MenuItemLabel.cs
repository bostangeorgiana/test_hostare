using System.ComponentModel.DataAnnotations.Schema;

namespace CampusEats.Persistence.Entities;

[Table("menu_item_labels")]
public class MenuItemLabel
{
    [Column("menu_item_id")]
    public int MenuItemId { get; set; }

    [ForeignKey(nameof(MenuItemId))]
    public MenuItem MenuItem { get; set; } = null!;

    [Column("label_id")]
    public int LabelId { get; set; }

    [ForeignKey(nameof(LabelId))]
    public MenuLabel Label { get; set; } = null!;
}