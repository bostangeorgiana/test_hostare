using CampusEats.Features.Auth.Login;
using CampusEats.Features.Auth.Logout;
using CampusEats.Features.Auth.RefreshToken;
using CampusEats.Features.Auth.Register;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace CampusEats.Features.Auth;

public static class AuthEndpoints
{
    public static IEndpointRouteBuilder MapAuthEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/auth")
            .WithTags("Auth");

        group.MapPost("/login", LoginEndpoint.Handle)
            .WithName("LoginUser")
            .WithOpenApi();

        group.MapPost("/register", RegisterEndpoint.Handle)
            .WithName("RegisterUser")
            .WithOpenApi();
        
        group.MapPost("/refresh", async ([FromServices] IMediator mediator) => await mediator.Send(new RefreshTokenCommand()))
            .WithName("RefreshToken")
            .WithOpenApi();

        group.MapPost("/logout", async ([FromServices] IMediator mediator) =>
            {
                return await mediator.Send(new LogoutCommand());
            })
            .RequireAuthorization()
            .WithName("LogoutUser")
            .WithOpenApi();


        return app;
    }
}