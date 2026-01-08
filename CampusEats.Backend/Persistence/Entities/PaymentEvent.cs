using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json;

namespace CampusEats.Persistence.Entities;

[Table("payment_events")]
public class PaymentEvent
{
    [Key]
    [Column("event_id")]
    public int EventId { get; set; }

    [Column("payment_id")]
    public int PaymentId { get; set; }

    [ForeignKey(nameof(PaymentId))]
    public Payment Payment { get; set; } = null!;

    [Required]
    [MaxLength(50)]
    [Column("event_type")]
    public string EventType { get; set; } = string.Empty;

    [Required]
    [MaxLength(20)]
    [Column("event_status")]
    public string EventStatus { get; set; } = string.Empty;

    [Column("message")]
    public string? Message { get; set; }

    [Column("payload", TypeName = "jsonb")]
    public string? Payload { get; set; }

    [Column("created_at")]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public void SetPayload(object data)
    {
        Payload = JsonSerializer.Serialize(data);
    }

    public T? GetPayload<T>()
    {
        return Payload == null
            ? default
            : JsonSerializer.Deserialize<T>(Payload);
    }
}