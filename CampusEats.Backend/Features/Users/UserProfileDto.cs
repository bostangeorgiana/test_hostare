namespace CampusEats.Features.Users;

public record UserProfileDto(
    int UserId,
    string FirstName,
    string LastName,
    string Email,
    string Role,
    int? LoyaltyPoints
);