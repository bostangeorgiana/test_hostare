using System.ComponentModel.DataAnnotations;

namespace CampusEats.Frontend.Models;

public class RegisterRequest
{
    [Required(ErrorMessage = "First name is required.")]
    [MaxLength(100, ErrorMessage = "First name must be at most 100 characters long.")]
    [RegularExpression("^[A-Z][a-zA-Z '-]*$", 
        ErrorMessage = "First name must start with a capital letter and contain only letters, spaces, hyphens or apostrophes.")]
    public string FirstName { get; set; } = "";

    [Required(ErrorMessage = "Last name is required.")]
    [MaxLength(100, ErrorMessage = "Last name must be at most 100 characters long.")]
    [RegularExpression("^[A-Z][a-zA-Z '-]*$", 
        ErrorMessage = "Last name must start with a capital letter and contain only letters, spaces, hyphens or apostrophes.")]
    public string LastName { get; set; } = "";

    [Required(ErrorMessage = "Email is required.")]
    [EmailAddress(ErrorMessage = "Invalid email address format.")]
    [RegularExpression(@"^[^@\s]+@[^@\s]+\.[^@\s]+$", 
        ErrorMessage = "Email must be a valid format (e.g. example@domain.com).")]
    [MaxLength(150)]
    public string Email { get; set; } = "";

    [Required(ErrorMessage = "Password is required.")]
    [MinLength(8, ErrorMessage = "Password must be at least 8 characters long.")]
    public string Password { get; set; } = "";

    [Required(ErrorMessage = "Password confirmation is required.")]
    [Compare("Password", ErrorMessage = "Passwords do not match.")]
    public string ConfirmPassword { get; set; } = "";
}