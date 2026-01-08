namespace CampusEats.Frontend.Features.Kitchen.Models;

public class OrderItemDto
{
    public int MenuItemId { get; set; }
    public string MenuItemName { get; set; } = "";
    public int Quantity { get; set; }
}