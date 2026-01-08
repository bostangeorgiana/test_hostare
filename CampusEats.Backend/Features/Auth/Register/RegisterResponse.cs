namespace CampusEats.Features.Auth.Register;

public record RegisterResponse(
    int UserId,
    string FirstName,
    string LastName,
    string Email,
    string Role
);