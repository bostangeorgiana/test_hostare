namespace CampusEats.Frontend.Features.Kitchen.Models;

public class OrderSummaryDto
{
    public int OrderId { get; set; }
    public string StudentName { get; set; } = "";
    public decimal TotalAmount { get; set; }
    public string Status { get; set; } = "";
    public DateTime CreatedAt { get; set; }
    public List<OrderItemDto> Items { get; set; } = new();
    public string? Notes { get; set; } 
}