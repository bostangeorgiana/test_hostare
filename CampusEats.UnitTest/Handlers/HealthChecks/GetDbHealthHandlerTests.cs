namespace CampusEats.UnitTest.Handlers.HealthChecks;

using System.Threading.Tasks;
using Xunit;
using CampusEats.UnitTest.Helpers;

public class GetDbHealthHandlerTests
{
    [Fact]
    public async Task Given_DbAvailable_When_Handle_Then_ReturnsHealthyTrue()
    {
        using var dbContext = ContextHelper.CreateInMemoryDbContext();
        var handler = new CampusEats.Features.HealthChecks.GetDbHealthHandler(dbContext);

        var result = await handler.Handle();

        var valueProp = result.GetType().GetProperty("Value");
        var valueObj = valueProp.GetValue(result);
        var healthyProp = valueObj.GetType().GetProperty("healthy");
        var healthy = (bool)healthyProp.GetValue(valueObj)!;

        Assert.True(healthy);
    }
}