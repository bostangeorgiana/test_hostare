using Microsoft.AspNetCore.Mvc;
namespace CampusEats.Features.Admin.ManageStudent;
public static class GetStudentsAdminEndpoint
{
    public static async Task<IResult> HandleList(
        [FromServices] GetStudentsAdminHandler handler,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 10,
        CancellationToken token = default)
    {
        var result = await handler.GetList(page, pageSize, token);
        return Results.Ok(result);
    }

    public static async Task<IResult> HandleById(
        int id,
        [FromServices] GetStudentsAdminHandler handler,
        CancellationToken token)
    {
        var result = await handler.GetById(id, token);

        return result.IsSuccess
            ? Results.Ok(result.Value)
            : Results.NotFound(result.Error);
    }
}