namespace CampusEats.UnitTest.Handlers.Loyalty;

using System.Threading;
using Xunit;
using FluentAssertions;
using CampusEats.UnitTest.Helpers;
using CampusEats.Persistence.Entities;
using CampusEats.Features.Loyalty.AddLoyaltyPoints;
using CampusEats.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;

public class AddLoyaltyPointsHandlerTests
{
    [Fact]
    public async System.Threading.Tasks.Task Given_ExistingStudent_When_HandleAsync_Then_PointsAdded()
    {
        using var dbContext = ContextHelper.CreateInMemoryDbContext();
        dbContext.Students.Add(new Student { StudentId = 1, LoyaltyPoints = 10 });
        await dbContext.SaveChangesAsync();
        
        var tracked = await dbContext.Students.FindAsync(1);
        if (tracked != null)
            dbContext.Entry(tracked).State = EntityState.Detached;

        var repo = new StudentRepository(dbContext);
        var handler = new CampusEats.Features.Loyalty.AddLoyaltyPoints.AddLoyaltyPointsHandler(repo);

        var request = new CampusEats.Features.Loyalty.AddLoyaltyPoints.AddLoyaltyPointsRequest
        {
            StudentId = 1,
            PointsToAdd = 5
        };

        var response = await handler.HandleAsync(request);

        response.Should().NotBeNull();
        response.StudentId.Should().Be(1);
        response.LoyaltyPoints.Should().Be(15);

        var updated = await dbContext.Students.FindAsync(1);
        updated.Should().NotBeNull();
        updated!.LoyaltyPoints.Should().Be(15);
    }

    [Fact]
    public async System.Threading.Tasks.Task Given_NonExistentStudent_When_HandleAsync_Then_ThrowsException()
    {
        using var dbContext = ContextHelper.CreateInMemoryDbContext();
        var repo = new StudentRepository(dbContext);
        var handler = new CampusEats.Features.Loyalty.AddLoyaltyPoints.AddLoyaltyPointsHandler(repo);

        var request = new CampusEats.Features.Loyalty.AddLoyaltyPoints.AddLoyaltyPointsRequest
        {
            StudentId = 999,
            PointsToAdd = 5
        };

        await Assert.ThrowsAsync<System.Exception>(() => handler.HandleAsync(request));
    }
}