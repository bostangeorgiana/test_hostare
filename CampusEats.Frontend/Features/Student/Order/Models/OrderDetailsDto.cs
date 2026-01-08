namespace CampusEats.Frontend.Features.Student.Order.Models;

public class OrderDetailsDto
{
    public int OrderId { get; set; }
    public decimal TotalAmount { get; set; }
    public string Status { get; set; } = "";
    public DateTime CreatedAt { get; set; }
    public List<OrderItemDetailsDto> Items { get; set; } = new();
    public int LoyaltyPointsUsed { get; set; }
    public int TotalLoyaltyPointsEarned { get; set; }
    public decimal FinalAmountPaid { get; set; }
}

public class OrderItemDetailsDto
{
    public int MenuItemId { get; set; }
    public string MenuItemName { get; set; } = "";
    public int Quantity { get; set; }
    public decimal Price { get; set; }
}