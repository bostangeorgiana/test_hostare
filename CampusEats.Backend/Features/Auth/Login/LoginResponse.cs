namespace CampusEats.Features.Auth.Login;

public record LoginResponse(
    string AccessToken,
    string TokenType,
    int ExpiresIn,
    LoginUserInfo User
);

public record LoginUserInfo(
    int UserId,
    string FirstName,
    string LastName,
    string Email,
    string Role
);