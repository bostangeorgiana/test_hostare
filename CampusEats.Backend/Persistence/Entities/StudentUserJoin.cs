namespace CampusEats.Persistence.Entities;

public class StudentUserJoin
{
    public int UserId { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public int LoyaltyPoints { get; set; }
    public string Role { get; set; } = "student";
}