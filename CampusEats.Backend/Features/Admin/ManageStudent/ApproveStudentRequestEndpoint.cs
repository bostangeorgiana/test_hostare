namespace CampusEats.Features.Admin.ManageStudent;

public static class ApproveStudentRequestEndpoint
{
    public static async Task<IResult> Handle(
        int id,
        ApproveStudentRequestHandler handler,
        CancellationToken cancellationToken)
    {
        var result = await handler.Handle(id, cancellationToken);

        return result.IsSuccess
            ? Results.Ok(new { message = "Student request approved successfully" })
            : Results.BadRequest(result.Error);
    }
}