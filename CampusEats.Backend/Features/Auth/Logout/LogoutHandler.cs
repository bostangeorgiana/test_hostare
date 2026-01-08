using System.Security.Claims;
using CampusEats.Persistence.Repositories;
using CampusEats.Shared;
using MediatR;

namespace CampusEats.Features.Auth.Logout;

public class LogoutHandler(
    UserRepository users,
    IHttpContextAccessor httpContext)
    : IRequestHandler<LogoutCommand, Result<bool>>
{
    public async Task<Result<bool>> Handle(
        LogoutCommand command,
        CancellationToken cancellationToken)
    {
        var http = httpContext.HttpContext!;
        
        var userIdClaim = http.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrEmpty(userIdClaim))
            return Result<bool>.Failure("User not authenticated");

        int userId = int.Parse(userIdClaim);
        
        await users.UpdateRefreshTokenAsync(userId, null!, DateTime.MinValue);
        
        http.Response.Cookies.Delete("refreshToken");

        return Result<bool>.Success(true);
    }
}