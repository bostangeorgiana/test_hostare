namespace CampusEats.Features.Auth.RefreshToken;

public record RefreshTokenResponse(string AccessToken, int ExpiresIn);