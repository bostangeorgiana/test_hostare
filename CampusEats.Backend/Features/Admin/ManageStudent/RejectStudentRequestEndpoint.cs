namespace CampusEats.Features.Admin.ManageStudent;

public static class RejectStudentRequestEndpoint
{
    public static async Task<IResult> Handle(
        int id,
        RejectStudentRequestHandler handler,
        CancellationToken cancellationToken)
    {
        var result = await handler.Handle(id, cancellationToken);
        
        return result.IsSuccess 
            ? Results.Ok(new { message = "Student request rejected successfully" })
            : Results.BadRequest(result.Error);
    }
}