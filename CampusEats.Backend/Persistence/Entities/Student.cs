using System.ComponentModel.DataAnnotations.Schema;

namespace CampusEats.Persistence.Entities;

[Table("students")]
public class Student
{
    [Column("student_id")]
    public int StudentId { get; set; }

    [Column("loyalty_points")]
    public int LoyaltyPoints { get; set; }

    [Column("reserved_points")]
    public int ReservedLoyaltyPoints { get; set; } = 0;
    
    [ForeignKey(nameof(StudentId))]
    public User User { get; set; } = null!;
    
    public ICollection<Favorite> Favorites { get; set; } = new List<Favorite>();
}