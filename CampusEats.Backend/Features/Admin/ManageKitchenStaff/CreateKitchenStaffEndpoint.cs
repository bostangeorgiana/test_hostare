using Microsoft.AspNetCore.Mvc;

namespace CampusEats.Features.Admin.ManageKitchenStaff;

public static class CreateKitchenStaffEndpoint
{
    public static async Task<IResult> Handle(
        [FromBody] CreateKitchenStaffCommand command,
        CreateKitchenStaffHandler handler,
        CancellationToken cancellationToken)
    {
        var result = await handler.Handle(command, cancellationToken);

        if (result.IsSuccess && result.Value is not null)
        {
            return Results.Created(
                $"/admin/kitchen-staff/{result.Value.UserId}",
                (object?)result.Value
            );
        }

        return Results.BadRequest((object?)result.Error);
    }
}