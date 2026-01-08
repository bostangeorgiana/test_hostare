namespace CampusEats.UnitTest.Handlers.Admin.ManageKitchenStaff;

using Xunit;
using FluentAssertions;
using CampusEats.UnitTest.Helpers;
using CampusEats.Persistence.Entities;
using CampusEats.Persistence.Repositories;
using CampusEats.Features.Admin.ManageKitchenStaff;
using Microsoft.EntityFrameworkCore;

public class DeleteKitchenStaffHandlerTests
{
    [Fact]
    public async System.Threading.Tasks.Task Given_UserIsNotKitchenStaff_When_Handle_Then_ReturnsFailure()
    {
        using var db = ContextHelper.CreateInMemoryDbContext();
        db.Users.Add(new User { UserId = 10, Email = "u1@example.com", FirstName = "U", LastName = "One", Password = "x", Role = "student" });
        await db.SaveChangesAsync();

        var users = new UserRepository(db);
        var handler = new DeleteKitchenStaffHandler(users);

        var result = await handler.Handle(10, System.Threading.CancellationToken.None);

        result.IsSuccess.Should().BeFalse();
        result.Error.Should().NotBeNullOrEmpty();
    }

    [Fact]
    public async System.Threading.Tasks.Task Given_UserNotFound_When_Handle_Then_ReturnsFailure()
    {
        using var db = ContextHelper.CreateInMemoryDbContext();
        var users = new UserRepository(db);
        var handler = new DeleteKitchenStaffHandler(users);

        var result = await handler.Handle(9999, System.Threading.CancellationToken.None);

        result.IsSuccess.Should().BeFalse();
        result.Error.Should().NotBeNullOrEmpty();
    }

    [Fact]
    public async System.Threading.Tasks.Task Given_KitchenStaffExists_When_Handle_Then_DeletesKitchenStaff()
    {
        using var db = ContextHelper.CreateInMemoryDbContext();
        db.Users.Add(new User { UserId = 20, Email = "chef@example.com", FirstName = "C", LastName = "Hef", Password = "x", Role = "kitchen_staff" });
        await db.SaveChangesAsync();

        var users = new UserRepository(db);
        var handler = new DeleteKitchenStaffHandler(users);

        var result = await handler.Handle(20, System.Threading.CancellationToken.None);

        result.IsSuccess.Should().BeTrue();

        var deleted = await db.Users.FindAsync(20);
        deleted.Should().BeNull();
    }
}