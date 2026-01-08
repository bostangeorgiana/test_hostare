using System.ComponentModel.DataAnnotations.Schema;

namespace CampusEats.Persistence.Entities;

[Table("ingredients")]
public class Ingredient
{
    [Column("ingredient_id")]
    public int IngredientId { get; set; }

    [Column("name")]
    public string Name { get; set; } = string.Empty;

    [Column("allergens")]
    public string? Allergens { get; set; }

    [Column("calories_per_unit")]
    public int CaloriesPerUnit { get; set; }
    
    public ICollection<MenuItemIngredient>? MenuItemIngredients { get; set; }
}