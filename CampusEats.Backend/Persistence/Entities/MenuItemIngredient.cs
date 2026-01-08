using System.ComponentModel.DataAnnotations.Schema;

namespace CampusEats.Persistence.Entities;

[Table("menu_item_ingredients")]
public class MenuItemIngredient
{
    [Column("menu_item_id")]
    public int MenuItemId { get; set; }

    [ForeignKey(nameof(MenuItemId))]
    public MenuItem MenuItem { get; set; } = null!;

    [Column("ingredient_id")]
    public int IngredientId { get; set; }

    [ForeignKey(nameof(IngredientId))]
    public Ingredient Ingredient { get; set; } = null!;

    [Column("quantity")]
    public decimal Quantity { get; set; }
}