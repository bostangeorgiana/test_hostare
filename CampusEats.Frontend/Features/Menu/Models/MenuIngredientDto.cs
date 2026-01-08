namespace CampusEats.Frontend.Features.Menu.Models
{
    public class MenuIngredientDto
    {
        public int IngredientId { get; set; }
        public string Name { get; set; } = "";
        public int? Quantity { get; set; }
    }
}