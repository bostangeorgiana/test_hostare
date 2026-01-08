using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CampusEats.Persistence.Entities;

[Table("payments")]
public class Payment
{
    [Key]
    [Column("payment_id")]
    public int PaymentId { get; set; }

    [Column("order_id")]
    public int OrderId { get; set; }

    [ForeignKey(nameof(OrderId))]
    public Order Order { get; set; } = null!;

    [Column("amount")]
    public decimal Amount { get; set; }


    [Column("currency")]
    public string Currency { get; set; } = "RON";

    [Column("payment_method")]
    public string? PaymentMethod { get; set; }

    [Column("payment_provider")]
    public string PaymentProvider { get; set; } = "mock";

    [Column("provider_payment_id")]
    public string? ProviderPaymentId { get; set; }

    [Column("payment_intent_secret")]
    public string? PaymentIntentSecret { get; set; }

    [Column("status")]
    public string Status { get; set; } = "pending";

    [Column("transaction_id")]
    public string? TransactionId { get; set; }

    [Column("confirmed_at")]
    public DateTime? ConfirmedAt { get; set; }

    [Column("created_at")]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    
    [Column("updated_at")]
    public DateTime? UpdatedAt { get; set; }
    
    public ICollection<PaymentEvent> PaymentEvents { get; set; } = new List<PaymentEvent>();
}