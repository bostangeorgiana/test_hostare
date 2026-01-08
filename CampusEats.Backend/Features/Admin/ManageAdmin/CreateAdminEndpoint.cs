using Microsoft.AspNetCore.Mvc;

namespace CampusEats.Features.Admin.ManageAdmin.CreateAdmin;

public static class CreateAdminEndpoint
{
    public static async Task<IResult> Handle(
        [FromBody] CreateAdminCommand command,
        CreateAdminHandler handler,
        CancellationToken cancellationToken)
    {
        var result = await handler.Handle(command, cancellationToken);
        
        if ((result.IsSuccess) && (result.Value is not null))
        {
            return Results.Created($"/admin/{result.Value.UserId}", result.Value);
        }

        return Results.BadRequest(result.Error);

    }
}