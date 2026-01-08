using CampusEats.Persistence.Repositories;

namespace CampusEats.Features.Users;

public static class UserEndpoints
{
    public static IEndpointRouteBuilder MapUserEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/users").WithTags("Users");

        group.MapGet("/{id}/profile", async (int id, StudentRepository students) =>
        {
            if (id <= 0) return Results.BadRequest("Invalid user id.");

            var info = await students.GetUserProfileAsync(id);
            if (info == null) return Results.NotFound();

            var isStudent = string.Equals(info.Role, "student", StringComparison.OrdinalIgnoreCase);

            var dto = new UserProfileDto(
                UserId: info.UserId,
                FirstName: info.FirstName,
                LastName: info.LastName,
                Email: info.Email,
                Role: info.Role,
                LoyaltyPoints: isStudent ? info.LoyaltyPoints : null
            );

            return Results.Ok(dto);
        })
        .WithName("GetUserProfile")
        .WithOpenApi();

        // Optionally expose a /me/profile route that returns the authenticated user's profile without an id:
        group.MapGet("/me/profile", async (HttpContext ctx, StudentRepository students) =>
        {
            var userIdClaim = ctx.User.FindFirst("sub")?.Value ?? ctx.User.FindFirst("user_id")?.Value;
            if (!int.TryParse(userIdClaim, out var userId)) return Results.Unauthorized();

            var info = await students.GetUserProfileAsync(userId);
            if (info == null) return Results.NotFound();

            var isStudent = string.Equals(info.Role, "student", StringComparison.OrdinalIgnoreCase);

            var dto = new UserProfileDto(
                UserId: info.UserId,
                FirstName: info.FirstName,
                LastName: info.LastName,
                Email: info.Email,
                Role: info.Role,
                LoyaltyPoints: isStudent ? info.LoyaltyPoints : null
            );

            return Results.Ok(dto);
        })
        .RequireAuthorization()
        .WithName("GetMyProfile")
        .WithOpenApi();

        return app;
    }
}
