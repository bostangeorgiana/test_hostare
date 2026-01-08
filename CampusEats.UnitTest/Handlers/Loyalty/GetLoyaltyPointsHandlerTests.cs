namespace CampusEats.UnitTest.Handlers.Loyalty;

using Xunit;
using FluentAssertions;
using CampusEats.UnitTest.Helpers;
using CampusEats.Persistence.Entities;
using CampusEats.Persistence.Repositories;

public class GetLoyaltyPointsHandlerTests
{
    [Fact]
    public async System.Threading.Tasks.Task Given_ExistingStudent_When_HandleAsync_Then_ReturnsPoints()
    {
        using var dbContext = ContextHelper.CreateInMemoryDbContext();
        dbContext.Students.Add(new Student { StudentId = 5, LoyaltyPoints = 42 });
        await dbContext.SaveChangesAsync();

        var repo = new StudentRepository(dbContext);
        var handler = new CampusEats.Features.Loyalty.GetLoyaltyPoints.GetLoyaltyPointsHandler(repo);

        var query = new CampusEats.Features.Loyalty.GetLoyaltyPoints.GetLoyaltyPointsQuery(5);

        var response = await handler.HandleAsync(query);

        response.Should().NotBeNull();
        response.StudentId.Should().Be(5);
        response.LoyaltyPoints.Should().Be(42);
    }

    [Fact]
    public async System.Threading.Tasks.Task Given_NonExistentStudent_When_HandleAsync_Then_Throws()
    {
        using var dbContext = ContextHelper.CreateInMemoryDbContext();
        var repo = new StudentRepository(dbContext);
        var handler = new CampusEats.Features.Loyalty.GetLoyaltyPoints.GetLoyaltyPointsHandler(repo);

        var query = new CampusEats.Features.Loyalty.GetLoyaltyPoints.GetLoyaltyPointsQuery(999);

        await Assert.ThrowsAsync<System.Exception>(() => handler.HandleAsync(query));
    }
}