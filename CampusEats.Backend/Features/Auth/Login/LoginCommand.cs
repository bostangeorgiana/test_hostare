namespace CampusEats.Features.Auth.Login;

/// <summary>
/// Request DTO for user login.
/// </summary>
/// <param name="Email">User's email address.</param>
/// <param name="Password">User's plaintext password.</param>
public record LoginCommand(string Email, string Password);