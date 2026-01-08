using CampusEats.Persistence.Repositories;
using CampusEats.Services.JWT;
using CampusEats.Shared;
using FluentValidation;

namespace CampusEats.Features.Auth.Login;

public class LoginHandler(
    UserRepository users,
    IJwtTokenService jwt,
    JwtOptions jwtOptions,
    IValidator<LoginCommand> validator,
    ILogger<LoginHandler> logger,
    IHttpContextAccessor httpContext)
{
    private const string InvalidCredentials = "Invalid email or password.";

    public async Task<Result<LoginResponse>> Handle(
        LoginCommand command,
        CancellationToken cancellationToken)
    {
        var validation = await validator.ValidateAsync(command, cancellationToken);
        if (!validation.IsValid)
        {
            var error = string.Join("; ", validation.Errors.Select(e => e.ErrorMessage));
            return Result<LoginResponse>.Failure(error);
        }

        var user = await users.GetByEmailAsync(command.Email);
        if (user is null)
        {
            logger.LogWarning("Login failed: user not found {Email}", command.Email);
            return Result<LoginResponse>.Failure(InvalidCredentials);
        }

        if (user.Role == "Pending")
            return Result<LoginResponse>.Failure("Your account is still pending approval.");

        var isValid = BCrypt.Net.BCrypt.Verify(command.Password, user.Password);
        if (!isValid)
        {
            logger.LogWarning("Login failed: wrong password {Email}", command.Email);
            return Result<LoginResponse>.Failure(InvalidCredentials);
        }
        
        var accessToken = jwt.CreateAccessToken(user.UserId, user.Email, user.Role);
        
        var refreshToken = jwt.CreateRefreshToken();
        var refreshExpiry = DateTime.UtcNow.AddDays(7);

        await users.UpdateRefreshTokenAsync(user.UserId, refreshToken, refreshExpiry);
        
        httpContext.HttpContext!.Response.Cookies.Append(
            "refreshToken",
            refreshToken,
            new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Strict,
                Expires = refreshExpiry
            }
        );
        
        var response = new LoginResponse(
            AccessToken: accessToken,
            TokenType: "Bearer",
            ExpiresIn: jwtOptions.ExpiresMinutes * 60,
            User: new LoginUserInfo(
                user.UserId,
                user.FirstName,
                user.LastName,
                user.Email,
                user.Role
            )
        );

        logger.LogInformation("User logged in: {Email}", command.Email);

        return Result<LoginResponse>.Success(response);
    }
}
