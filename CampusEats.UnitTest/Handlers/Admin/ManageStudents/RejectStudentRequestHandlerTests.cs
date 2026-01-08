namespace CampusEats.UnitTest.Handlers.Admin.ManageStudents;

using Xunit;
using FluentAssertions;
using CampusEats.UnitTest.Helpers;
using CampusEats.Persistence.Entities;
using CampusEats.Persistence.Repositories;
using CampusEats.Features.Admin.ManageStudent;
using Microsoft.EntityFrameworkCore;

public class RejectStudentRequestHandlerTests
{
    [Fact]
    public async System.Threading.Tasks.Task Given_UserNotFound_When_Handle_Then_ReturnsFailure()
    {
        using var db = ContextHelper.CreateInMemoryDbContext();
        var users = new UserRepository(db);
        var handler = new RejectStudentRequestHandler(users);

        var result = await handler.Handle(9999, System.Threading.CancellationToken.None);

        result.IsSuccess.Should().BeFalse();
        result.Error.Should().NotBeNullOrEmpty();
    }

    [Fact]
    public async System.Threading.Tasks.Task Given_UserNotPending_When_Handle_Then_ReturnsFailure()
    {
        using var db = ContextHelper.CreateInMemoryDbContext();
        db.Users.Add(new User { UserId = 2, Email = "u2@example.com", FirstName = "U", LastName = "Two", Password = "x", Role = "student" });
        await db.SaveChangesAsync();

        var users = new UserRepository(db);
        var handler = new RejectStudentRequestHandler(users);

        var result = await handler.Handle(2, System.Threading.CancellationToken.None);

        result.IsSuccess.Should().BeFalse();
        result.Error.Should().NotBeNullOrEmpty();
    }

    [Fact]
    public async System.Threading.Tasks.Task Given_PendingUser_When_Handle_Then_DeletesUser()
    {
        using var db = ContextHelper.CreateInMemoryDbContext();
        db.Users.Add(new User { UserId = 3, Email = "pending@example.com", FirstName = "P", LastName = "User", Password = "x", Role = "Pending" });
        await db.SaveChangesAsync();

        var users = new UserRepository(db);
        var handler = new RejectStudentRequestHandler(users);

        var result = await handler.Handle(3, System.Threading.CancellationToken.None);

        result.IsSuccess.Should().BeTrue();

        var deleted = await db.Users.FindAsync(3);
        deleted.Should().BeNull();
    }
}