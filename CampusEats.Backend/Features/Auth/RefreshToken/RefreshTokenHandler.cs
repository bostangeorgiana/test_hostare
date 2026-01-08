using CampusEats.Persistence.Repositories;
using CampusEats.Services.JWT;
using CampusEats.Shared;
using MediatR;

namespace CampusEats.Features.Auth.RefreshToken;

public class RefreshTokenHandler(
    UserRepository users,
    IJwtTokenService jwt,
    JwtOptions jwtOptions,
    IHttpContextAccessor httpContext)
    : IRequestHandler<RefreshTokenCommand, Result<RefreshTokenResponse>>
{
    public async Task<Result<RefreshTokenResponse>> Handle(
        RefreshTokenCommand command,
        CancellationToken cancellationToken)
    {
        var refreshToken = httpContext.HttpContext!.Request.Cookies["refreshToken"];
        if (string.IsNullOrEmpty(refreshToken))
            return Result<RefreshTokenResponse>.Failure("Refresh token missing.");
        
        var user = await users.GetByRefreshTokenAsync(refreshToken);
        if (user == null)
            return Result<RefreshTokenResponse>.Failure("Invalid or expired refresh token.");
        
        var newAccessToken = jwt.CreateAccessToken(user.UserId, user.Email, user.Role);
        var newRefreshToken = jwt.CreateRefreshToken();
        var refreshExpiry = DateTime.UtcNow.AddDays(7);
        
        await users.UpdateRefreshTokenAsync(user.UserId, newRefreshToken, refreshExpiry);
        
        httpContext.HttpContext.Response.Cookies.Append(
            "refreshToken",
            newRefreshToken,
            new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Strict,
                Expires = refreshExpiry
            }
        );
        
        var response = new RefreshTokenResponse(
            AccessToken: newAccessToken,
            ExpiresIn: jwtOptions.ExpiresMinutes * 60
        );

        return Result<RefreshTokenResponse>.Success(response);
    }
}
