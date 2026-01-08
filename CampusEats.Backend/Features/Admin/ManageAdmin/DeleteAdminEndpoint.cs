namespace CampusEats.Features.Admin.ManageAdmin;

public static class DeleteAdminEndpoint
{
    public static async Task<IResult> Handle(
        int id,
        DeleteAdminHandler handler,
        CancellationToken cancellationToken)
    {
        var result = await handler.Handle(id, cancellationToken);
        
        return result.IsSuccess 
            ? Results.NoContent()
            : Results.BadRequest(result.Error);
    }
}