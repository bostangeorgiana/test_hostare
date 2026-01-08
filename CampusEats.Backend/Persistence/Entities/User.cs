using System.ComponentModel.DataAnnotations.Schema;

namespace CampusEats.Persistence.Entities;

[Table("users")]
public class User
{
    [Column("user_id")]
    public int UserId { get; set; }

    [Column("first_name")]
    public string FirstName { get; set; } = string.Empty;

    [Column("last_name")]
    public string LastName { get; set; } = string.Empty;

    [Column("email")]
    public string Email { get; set; } = string.Empty;

    [Column("password")]
    public string Password { get; set; } = string.Empty;

    [Column("role")]
    public string Role { get; set; } = "student";

    [Column("refresh_token")]
    public string? RefreshToken { get; set; }

    [Column("refresh_token_expires_at")]
    public DateTime? RefreshTokenExpiresAt { get; set; }
    
    public Student? Student { get; set; }
}