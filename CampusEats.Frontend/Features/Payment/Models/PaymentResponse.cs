namespace CampusEats.Frontend.Features.Payment.Models;

public class PaymentResponse
{
    public bool Success { get; set; }
    public string Message { get; set; } = "";
    public decimal Amount { get; set; }
}