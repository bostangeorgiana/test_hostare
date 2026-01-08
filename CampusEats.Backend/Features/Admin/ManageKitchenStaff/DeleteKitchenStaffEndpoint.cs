namespace CampusEats.Features.Admin.ManageKitchenStaff;

public static class DeleteKitchenStaffEndpoint
{
    public static async Task<IResult> Handle(
        int id,
        DeleteKitchenStaffHandler handler,
        CancellationToken cancellationToken)
    {
        var result = await handler.Handle(id, cancellationToken);

        return result.IsSuccess
            ? Results.NoContent()
            : Results.BadRequest(new { error = result.Error });
    }
}