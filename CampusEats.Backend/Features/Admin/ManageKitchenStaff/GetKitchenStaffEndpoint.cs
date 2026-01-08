using Microsoft.AspNetCore.Mvc;

namespace CampusEats.Features.Admin.ManageKitchenStaff;

public static class GetKitchenStaffEndpoint
{
    public static async Task<IResult> Handle(
        GetKitchenStaffHandler handler,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 10,
        CancellationToken cancellationToken = default)
    {
        var result = await handler.Handle(page, pageSize, cancellationToken);

        return Results.Ok(result);
    }
}