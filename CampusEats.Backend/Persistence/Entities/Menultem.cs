using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CampusEats.Persistence.Entities;

[Table("menu_items")]
public class MenuItem
{
    [Column("menu_item_id")]
    public int MenuItemId { get; set; }

    [Column("name")]
    [StringLength(80)]
    public string Name { get; set; } = string.Empty;

    [Column("description")]
    [StringLength(300)]
    public string? Description { get; set; }

    [Column("price")]
    public decimal Price { get; set; }

    [Column("calories")]
    public int Calories { get; set; }

    [Column("current_stock")]
    public int CurrentStock { get; set; }

    [Column("is_available")]
    public bool IsAvailable { get; set; } = true;

    [Column("picture_link")]
    [StringLength(255)]
    public string? PictureLink { get; set; }

    public ICollection<MenuItemLabel> MenuItemLabels { get; set; } = new List<MenuItemLabel>();
    public ICollection<MenuItemIngredient> MenuItemIngredients { get; set; } = new List<MenuItemIngredient>();
}