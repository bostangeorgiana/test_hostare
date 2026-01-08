using Microsoft.AspNetCore.Mvc;

namespace CampusEats.Features.Admin.ManageAdmin;

public static class GetAdminsEndpoint
{
    public static async Task<IResult> Handle(
        [FromServices] GetAdminsHandler handler,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 10,
        CancellationToken cancellationToken = default)
    {
        var request = new GetAdminsQuery
        {
            Page = page,
            PageSize = pageSize
        };

        var result = await handler.Handle(request, cancellationToken);

        return result.IsSuccess
            ? Results.Ok(result.Value)
            : Results.BadRequest(result.Error);
    }
}