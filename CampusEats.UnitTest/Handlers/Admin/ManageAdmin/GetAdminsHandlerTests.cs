namespace CampusEats.UnitTest.Handlers.Admin.Manage_Admin;

using Xunit;
using FluentAssertions;
using CampusEats.UnitTest.Helpers;
using CampusEats.Persistence.Entities;
using CampusEats.Persistence.Repositories;
using CampusEats.Features.Admin.ManageAdmin;
using Microsoft.EntityFrameworkCore;

public class GetAdminsHandlerTests
{
    [Fact]
    public async System.Threading.Tasks.Task Given_InvalidPagination_When_Handle_Then_ReturnsFailure()
    {
        var handler = new GetAdminsHandler(new UserRepository(ContextHelper.CreateInMemoryDbContext()));

        var query = new GetAdminsQuery { Page = 1, PageSize = 7 }; // not multiple of 10 -> invalid

        var result = await handler.Handle(query, System.Threading.CancellationToken.None);

        result.IsSuccess.Should().BeFalse();
        result.Error.Should().NotBeNullOrEmpty();
    }

    [Fact]
    public async System.Threading.Tasks.Task Given_NoAdmins_When_Handle_Then_ReturnsEmptyPage()
    {
        using var db = ContextHelper.CreateInMemoryDbContext();

        var users = new UserRepository(db);
        var handler = new GetAdminsHandler(users);

        var query = new GetAdminsQuery { Page = 1, PageSize = 10 };

        var result = await handler.Handle(query, System.Threading.CancellationToken.None);

        result.IsSuccess.Should().BeTrue();
        result.Value.Should().NotBeNull();
        result.Value.Items.Should().BeEmpty();
        result.Value.TotalCount.Should().Be(0);
        result.Value.Page.Should().Be(1);
        result.Value.PageSize.Should().Be(10);
        result.Value.TotalPages.Should().Be(0);
    }

    [Fact]
    public async System.Threading.Tasks.Task Given_AdminsExist_When_Handle_Then_ReturnsPagedAdmins()
    {
        using var db = ContextHelper.CreateInMemoryDbContext();

        // Seed 15 admins so we have multiple pages (pageSize 10)
        for (int i = 1; i <= 15; i++)
        {
            db.Users.Add(new User { UserId = i, Email = $"admin{i}@example.com", FirstName = "A", LastName = i.ToString(), Password = "x", Role = "admin" });
        }
        await db.SaveChangesAsync();

        var users = new UserRepository(db);
        var handler = new GetAdminsHandler(users);

        // Page 1
        var query1 = new GetAdminsQuery { Page = 1, PageSize = 10 };
        var r1 = await handler.Handle(query1, System.Threading.CancellationToken.None);
        r1.IsSuccess.Should().BeTrue();
        r1.Value.Items.Count.Should().Be(10);
        r1.Value.TotalCount.Should().Be(15);
        r1.Value.TotalPages.Should().Be(2);
        r1.Value.HasNextPage.Should().BeTrue();
        r1.Value.HasPreviousPage.Should().BeFalse();

        // Page 2
        var query2 = new GetAdminsQuery { Page = 2, PageSize = 10 };
        var r2 = await handler.Handle(query2, System.Threading.CancellationToken.None);
        r2.IsSuccess.Should().BeTrue();
        r2.Value.Items.Count.Should().Be(5);
        r2.Value.Page.Should().Be(2);
        r2.Value.HasNextPage.Should().BeFalse();
        r2.Value.HasPreviousPage.Should().BeTrue();
    }
}