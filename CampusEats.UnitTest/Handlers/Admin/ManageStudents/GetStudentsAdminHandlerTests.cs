namespace CampusEats.UnitTest.Handlers.Admin.ManageStudents;

using Xunit;
using FluentAssertions;
using CampusEats.UnitTest.Helpers;
using CampusEats.Persistence.Entities;
using CampusEats.Persistence.Repositories;
using CampusEats.Features.Admin.ManageStudent;
using Microsoft.EntityFrameworkCore;

public class GetStudentsAdminHandlerTests
{
    [Fact]
    public async System.Threading.Tasks.Task Given_InvalidPagination_When_GetList_Then_Throws()
    {
        var handler = new GetStudentsAdminHandler(new StudentRepository(ContextHelper.CreateInMemoryDbContext()));

        await Assert.ThrowsAsync<System.ArgumentException>(async () => await handler.GetList(1, 7, System.Threading.CancellationToken.None));
    }

    [Fact]
    public async System.Threading.Tasks.Task Given_NoStudents_When_GetList_Then_ReturnsEmpty()
    {
        using var db = ContextHelper.CreateInMemoryDbContext();

        var students = new StudentRepository(db);
        var handler = new GetStudentsAdminHandler(students);

        var response = await handler.GetList(1, 10, System.Threading.CancellationToken.None);

        response.Should().NotBeNull();
        response.Items.Should().BeEmpty();
        response.TotalCount.Should().Be(0);
        response.Page.Should().Be(1);
        response.PageSize.Should().Be(10);
        response.TotalPages.Should().Be(0);
    }

    [Fact]
    public async System.Threading.Tasks.Task Given_StudentsExist_When_GetList_Then_ReturnsPagedResults()
    {
        using var db = ContextHelper.CreateInMemoryDbContext();
        
        for (int i = 1; i <= 14; i++)
        {
            db.Users.Add(new User { UserId = i, Email = $"s{i}@example.com", FirstName = "S", LastName = i.ToString(), Password = "x", Role = "student" });
            db.Students.Add(new Student { StudentId = i, LoyaltyPoints = i * 10 });
        }
        await db.SaveChangesAsync();

        var students = new StudentRepository(db);
        var handler = new GetStudentsAdminHandler(students);

        var r1 = await handler.GetList(1, 10, System.Threading.CancellationToken.None);
        r1.Items.Count.Should().Be(10);
        r1.TotalCount.Should().Be(14);
        r1.TotalPages.Should().Be(2);
        r1.HasNextPage.Should().BeTrue();
        r1.HasPreviousPage.Should().BeFalse();

        var first = r1.Items.First();
        first.Email.Should().NotBeNullOrEmpty();
        first.LoyaltyPoints.Should().BeGreaterThanOrEqualTo(0);

        var r2 = await handler.GetList(2, 10, System.Threading.CancellationToken.None);
        r2.Items.Count.Should().Be(4);
        r2.Page.Should().Be(2);
        r2.HasNextPage.Should().BeFalse();
        r2.HasPreviousPage.Should().BeTrue();
    }
}