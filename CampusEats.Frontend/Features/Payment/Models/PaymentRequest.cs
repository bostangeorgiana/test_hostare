namespace CampusEats.Frontend.Features.Payment.Models;

public class PaymentRequest
{
    public int OrderId { get; set; }
    public string PaymentMethodId { get; set; } = "pm_card_visa";
    public int PointsToRedeem { get; set; } = 0;
}