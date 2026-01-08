namespace CampusEats.UnitTest.Handlers.Admin.ManageKitchenStaff;

using Xunit;
using FluentAssertions;
using CampusEats.UnitTest.Helpers;
using CampusEats.Persistence.Entities;
using CampusEats.Persistence.Repositories;
using CampusEats.Features.Admin.ManageKitchenStaff;
using Microsoft.EntityFrameworkCore;

public class CreateKitchenStaffHandlerTests
{
    [Fact]
    public async System.Threading.Tasks.Task Given_DuplicateEmail_When_Handle_Then_ReturnsFailure()
    {
        using var db = ContextHelper.CreateInMemoryDbContext();
        db.Users.Add(new User { UserId = 1, Email = "dup@example.com", FirstName = "D", LastName = "U", Password = "x", Role = "kitchen_staff" });
        await db.SaveChangesAsync();

        var users = new UserRepository(db);
        var handler = new CreateKitchenStaffHandler(users);

        var command = new CreateKitchenStaffCommand
        {
            FirstName = "New",
            LastName = "Staff",
            Email = "dup@example.com",
            Password = "password123"
        };

        var result = await handler.Handle(command, System.Threading.CancellationToken.None);

        result.IsSuccess.Should().BeFalse();
        result.Error.Should().NotBeNullOrEmpty();
    }

    [Fact]
    public async System.Threading.Tasks.Task Given_ValidCommand_When_Handle_Then_CreatesKitchenStaff()
    {
        using var db = ContextHelper.CreateInMemoryDbContext();
        var users = new UserRepository(db);
        var handler = new CreateKitchenStaffHandler(users);

        var command = new CreateKitchenStaffCommand
        {
            FirstName = "Mark",
            LastName = "Chef",
            Email = "mark.chef@example.com",
            Password = "StrongPass1"
        };

        var result = await handler.Handle(command, System.Threading.CancellationToken.None);

        result.IsSuccess.Should().BeTrue();
        result.Value.Should().NotBeNull();
        result.Value.Email.Should().Be("mark.chef@example.com");
        result.Value.Role.Should().Be("kitchen_staff");

        var created = await db.Users.FirstOrDefaultAsync(u => u.Email == "mark.chef@example.com");
        created.Should().NotBeNull();
        created!.Role.Should().Be("kitchen_staff");
        created.Password.Should().NotBe("StrongPass1");
    }
}

