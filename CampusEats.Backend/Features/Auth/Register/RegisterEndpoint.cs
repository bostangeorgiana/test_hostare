namespace CampusEats.Features.Auth.Register;

public static class RegisterEndpoint
{
    public static async Task<IResult> Handle(
        RegisterCommand command,
        RegisterHandler handler,
        CancellationToken cancellationToken)
    {
        var result = await handler.Handle(command, cancellationToken);

        return result.IsSuccess
            ? Results.Created($"/users/{result.Value!.UserId}", result.Value)
            : Results.BadRequest(new { errors = result.Error });
    }
}