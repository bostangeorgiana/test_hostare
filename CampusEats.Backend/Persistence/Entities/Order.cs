using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CampusEats.Persistence.Entities;

[Table("orders")]
public class Order
{
    [Key]
    [Column("order_id")]
    public int OrderId { get; set; }

    [Column("student_id")]
    public int StudentId { get; set; }

    [ForeignKey(nameof(StudentId))]
    public Student Student { get; set; } = null!;

    [Column("status")]
    public string Status { get; set; } = "pending";

    [Column("total_amount")]
    public decimal TotalAmount { get; set; }

    [Column("created_at")]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    [Column("updated_at")]
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    public List<OrderItem> OrderItems { get; set; } = new();

    [Column("loyalty_points_used")]
    public int LoyaltyPointsUsed { get; set; } = 0;

    [Column("total_loyalty_points_earned")]
    public int TotalLoyaltyPointsEarned { get; set; } = 0;

    [Column("final_amount_paid")]
    public decimal FinalAmountPaid { get; set; } = 0;

    [Column("notes")]
    public string? Notes { get; set; }
}