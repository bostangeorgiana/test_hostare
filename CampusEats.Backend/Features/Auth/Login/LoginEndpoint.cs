using Microsoft.AspNetCore.Mvc;

namespace CampusEats.Features.Auth.Login;

public static class LoginEndpoint
{
    public static async Task<IResult> Handle(
        [FromBody] LoginCommand command,
        [FromServices] LoginHandler handler,
        CancellationToken cancellationToken)
    {
        var result = await handler.Handle(command, cancellationToken);

        return result.IsSuccess
            ? Results.Ok(result.Value)
            : Results.BadRequest(result.Error);
            //: Results.Unauthorized();
    }
}