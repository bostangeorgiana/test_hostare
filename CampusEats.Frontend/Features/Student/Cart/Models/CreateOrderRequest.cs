namespace CampusEats.Frontend.Models;

public class CreateOrderRequest
{
    public int StudentId { get; set; }
    public List<CreateOrderItem> Items { get; set; } = new();
    public string? Notes { get; set; }
}

public class CreateOrderItem
{
    public int MenuItemId { get; set; }
    public int Quantity { get; set; }
}