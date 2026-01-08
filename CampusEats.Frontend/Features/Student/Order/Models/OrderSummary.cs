namespace CampusEats.Frontend.Features.Student.Order.Models;

public class OrderSummary
{
    public int OrderId { get; set; }
    public decimal TotalAmount { get; set; }
    public string Status { get; set; } = "";
    public DateTime CreatedAt { get; set; }
}