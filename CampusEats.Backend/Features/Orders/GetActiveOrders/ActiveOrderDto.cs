namespace CampusEats.Features.Orders.GetActiveOrders;

public class ActiveOrderDto
{
    public int OrderId { get; set; }
    public string StudentName { get; set; } = "";
    public decimal TotalAmount { get; set; }
    public string Status { get; set; } = "";
    public DateTime CreatedAt { get; set; }
    public List<ActiveOrderItemDto> Items { get; set; } = new();
    public string? Notes { get; set; } 
}

public class ActiveOrderItemDto
{
    public int MenuItemId { get; set; }
    public string MenuItemName { get; set; } = "";
    public int Quantity { get; set; }
}