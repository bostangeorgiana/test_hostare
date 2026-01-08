namespace CampusEats.UnitTest.Handlers.Admin.ManageStudents;

using Xunit;
using FluentAssertions;
using CampusEats.UnitTest.Helpers;
using CampusEats.Persistence.Entities;
using CampusEats.Persistence.Repositories;
using CampusEats.Features.Admin.ManageStudent;
using Microsoft.EntityFrameworkCore;

public class GetStudentRequestsHandlerTests
{
    [Fact]
    public async System.Threading.Tasks.Task Given_InvalidPagination_When_Handle_Then_Throws()
    {
        var handler = new GetStudentRequestsHandler(new UserRepository(ContextHelper.CreateInMemoryDbContext()));
        
        await Assert.ThrowsAsync<System.ArgumentException>(async () => await handler.Handle(1, 7, System.Threading.CancellationToken.None));
    }

    [Fact]
    public async System.Threading.Tasks.Task Given_NoPendingRequests_When_Handle_Then_ReturnsEmpty()
    {
        using var db = ContextHelper.CreateInMemoryDbContext();

        var users = new UserRepository(db);
        var handler = new GetStudentRequestsHandler(users);

        var response = await handler.Handle(1, 10, System.Threading.CancellationToken.None);

        response.Should().NotBeNull();
        response.Items.Should().BeEmpty();
        response.TotalCount.Should().Be(0);
        response.Page.Should().Be(1);
        response.PageSize.Should().Be(10);
        response.TotalPages.Should().Be(0);
        response.HasNextPage.Should().BeFalse();
        response.HasPreviousPage.Should().BeFalse();
    }

    [Fact]
    public async System.Threading.Tasks.Task Given_PendingRequestsExist_When_Handle_Then_ReturnsPagedResults()
    {
        using var db = ContextHelper.CreateInMemoryDbContext();

        // Seed 13 pending users plus some other roles
        for (int i = 1; i <= 13; i++)
        {
            db.Users.Add(new User { UserId = i, Email = $"p{i}@example.com", FirstName = "P", LastName = i.ToString(), Password = "x", Role = "Pending" });
        }
        db.Users.Add(new User { UserId = 100, Email = "admin@example.com", FirstName = "A", LastName = "D", Password = "x", Role = "admin" });
        await db.SaveChangesAsync();

        var users = new UserRepository(db);
        var handler = new GetStudentRequestsHandler(users);

        var r1 = await handler.Handle(1, 10, System.Threading.CancellationToken.None);
        r1.Items.Count.Should().Be(10);
        r1.TotalCount.Should().Be(13);
        r1.TotalPages.Should().Be(2);
        r1.HasNextPage.Should().BeTrue();
        r1.HasPreviousPage.Should().BeFalse();

        // Ensure mapping includes status and correct fields
        var firstItem = r1.Items.First();
        firstItem.Status.Should().Be("Pending");
        firstItem.Email.Should().NotBeNullOrEmpty();

        var r2 = await handler.Handle(2, 10, System.Threading.CancellationToken.None);
        r2.Items.Count.Should().Be(3);
        r2.Page.Should().Be(2);
        r2.HasNextPage.Should().BeFalse();
        r2.HasPreviousPage.Should().BeTrue();
    }
}