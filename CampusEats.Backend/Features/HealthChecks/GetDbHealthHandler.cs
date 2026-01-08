using CampusEats.Persistence.Context;
namespace CampusEats.Features.HealthChecks;

public class GetDbHealthHandler(CampusEatsDbContext db)
{
    public async Task<IResult> Handle()
    {
        var canConnect = await db.Database.CanConnectAsync();
        return Results.Ok(new { healthy = canConnect });
    }
}