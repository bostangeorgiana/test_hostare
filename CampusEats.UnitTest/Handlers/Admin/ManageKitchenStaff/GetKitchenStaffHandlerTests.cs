namespace CampusEats.UnitTest.Handlers.Admin.ManageKitchenStaff;

using Xunit;
using FluentAssertions;
using CampusEats.UnitTest.Helpers;
using CampusEats.Persistence.Entities;
using CampusEats.Persistence.Repositories;
using CampusEats.Features.Admin.ManageKitchenStaff;
using Microsoft.EntityFrameworkCore;

public class GetKitchenStaffHandlerTests
{
    [Fact]
    public async System.Threading.Tasks.Task Given_InvalidPagination_When_Handle_Then_ReturnsFailure()
    {
        var handler = new GetKitchenStaffHandler(new UserRepository(ContextHelper.CreateInMemoryDbContext()));
        
        await Assert.ThrowsAsync<System.ArgumentException>(async () => await handler.Handle(1, 7, System.Threading.CancellationToken.None));
    }

    [Fact]
    public async System.Threading.Tasks.Task Given_NoKitchenStaff_When_Handle_Then_ReturnsEmptyPage()
    {
        using var db = ContextHelper.CreateInMemoryDbContext();

        var users = new UserRepository(db);
        var handler = new GetKitchenStaffHandler(users);

        var response = await handler.Handle(1, 10, System.Threading.CancellationToken.None);

        response.Should().NotBeNull();
        response.Items.Should().BeEmpty();
        response.TotalCount.Should().Be(0);
        response.Page.Should().Be(1);
        response.PageSize.Should().Be(10);
        response.TotalPages.Should().Be(0);
    }

    [Fact]
    public async System.Threading.Tasks.Task Given_KitchenStaffExist_When_Handle_Then_ReturnsPagedKitchenStaff()
    {
        using var db = ContextHelper.CreateInMemoryDbContext();

        // Seed 12 kitchen staff so we have multiple pages
        for (int i = 1; i <= 12; i++)
        {
            db.Users.Add(new User { UserId = i, Email = $"ks{i}@example.com", FirstName = "K", LastName = i.ToString(), Password = "x", Role = "kitchen_staff" });
        }
        await db.SaveChangesAsync();

        var users = new UserRepository(db);
        var handler = new GetKitchenStaffHandler(users);

        var r1 = await handler.Handle(1, 10, System.Threading.CancellationToken.None);
        r1.Items.Count.Should().Be(10);
        r1.TotalCount.Should().Be(12);
        r1.TotalPages.Should().Be(2);
        r1.HasNextPage.Should().BeTrue();
        r1.HasPreviousPage.Should().BeFalse();

        var r2 = await handler.Handle(2, 10, System.Threading.CancellationToken.None);
        r2.Items.Count.Should().Be(2);
        r2.Page.Should().Be(2);
        r2.HasNextPage.Should().BeFalse();
        r2.HasPreviousPage.Should().BeTrue();
    }
}