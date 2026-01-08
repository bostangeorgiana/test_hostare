using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CampusEats.Persistence.Entities;

[Table("menu_labels")]
public class MenuLabel
{
    [Key]
    [Column("label_id")]
    public int LabelId { get; set; }

    [Column("name")]
    public string Name { get; set; } = string.Empty;

    public ICollection<MenuItemLabel> MenuItemLabels { get; set; } = new List<MenuItemLabel>();
}