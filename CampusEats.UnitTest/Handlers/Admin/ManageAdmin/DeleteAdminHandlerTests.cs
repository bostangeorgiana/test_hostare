namespace CampusEats.UnitTest.Handlers.Admin.Manage_Admin;

using Xunit;
using FluentAssertions;
using CampusEats.UnitTest.Helpers;
using CampusEats.Persistence.Entities;
using CampusEats.Persistence.Repositories;
using CampusEats.Features.Admin.ManageAdmin;
using Microsoft.EntityFrameworkCore;

public class DeleteAdminHandlerTests
{
    [Fact]
    public async System.Threading.Tasks.Task Given_UserIsNotAdmin_When_Handle_Then_ReturnsFailure()
    {
        using var db = ContextHelper.CreateInMemoryDbContext();
        db.Users.Add(new User { UserId = 10, Email = "u1@example.com", FirstName = "U", LastName = "One", Password = "x", Role = "student" });
        await db.SaveChangesAsync();

        var users = new UserRepository(db);
        var handler = new DeleteAdminHandler(users);

        var result = await handler.Handle(10, System.Threading.CancellationToken.None);

        result.IsSuccess.Should().BeFalse();
        result.Error.Should().NotBeNullOrEmpty();
    }

    [Fact]
    public async System.Threading.Tasks.Task Given_LastAdmin_When_Handle_Then_ReturnsFailure()
    {
        using var db = ContextHelper.CreateInMemoryDbContext();
        db.Users.Add(new User { UserId = 11, Email = "admin1@example.com", FirstName = "A", LastName = "One", Password = "x", Role = "admin" });
        await db.SaveChangesAsync();

        var users = new UserRepository(db);
        var handler = new DeleteAdminHandler(users);

        var result = await handler.Handle(11, System.Threading.CancellationToken.None);

        result.IsSuccess.Should().BeFalse();
        result.Error.Should().NotBeNullOrEmpty();
    }

    [Fact]
    public async System.Threading.Tasks.Task Given_MultipleAdmins_When_Handle_Then_DeletesAdmin()
    {
        using var db = ContextHelper.CreateInMemoryDbContext();
        db.Users.Add(new User { UserId = 20, Email = "adminA@example.com", FirstName = "A", LastName = "A", Password = "x", Role = "admin" });
        db.Users.Add(new User { UserId = 21, Email = "adminB@example.com", FirstName = "B", LastName = "B", Password = "x", Role = "admin" });
        await db.SaveChangesAsync();

        var users = new UserRepository(db);
        var handler = new DeleteAdminHandler(users);

        var result = await handler.Handle(20, System.Threading.CancellationToken.None);

        result.IsSuccess.Should().BeTrue();

        var remaining = await db.Users.CountAsync(u => u.Role == "admin");
        remaining.Should().Be(1);

        var deleted = await db.Users.FindAsync(20);
        deleted.Should().BeNull();
    }
}