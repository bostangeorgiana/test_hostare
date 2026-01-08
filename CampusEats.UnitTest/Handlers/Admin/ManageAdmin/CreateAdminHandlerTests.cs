namespace CampusEats.UnitTest.Handlers.Admin.Manage_Admin;

using Moq;
using Xunit;
using FluentAssertions;
using CampusEats.UnitTest.Helpers;
using CampusEats.Persistence.Repositories;
using CampusEats.Persistence.Entities;
using CampusEats.Features.Admin.ManageAdmin.CreateAdmin;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using CampusEats.Shared.Exceptions;

public class CreateAdminHandlerTests
{
    [Fact]
    public async System.Threading.Tasks.Task Given_InvalidModel_When_Handle_Then_ReturnsFailure()
    {
        using var db = ContextHelper.CreateInMemoryDbContext();
        db.Users.Add(new User { UserId = 1, Email = "bademail", FirstName = "F", LastName = "L", Password = "x", Role = "admin" });
        await db.SaveChangesAsync();

        var users = new UserRepository(db);
        var handler = new CreateAdminHandler(users);

        var command = new CreateAdminCommand
        {
            FirstName = "First",
            LastName = "Last",
            Email = "bademail",
            Password = "pwd"
        };

        var result = await handler.Handle(command, System.Threading.CancellationToken.None);

        result.IsSuccess.Should().BeFalse();
        result.Error.Should().NotBeNullOrEmpty();
    }

    [Fact]
    public async System.Threading.Tasks.Task Given_ValidCommand_When_Handle_Then_AdminIsCreated()
    {
        using var db = ContextHelper.CreateInMemoryDbContext();
        var users = new UserRepository(db);

        // No validator required here because CreateAdminHandler doesn't take one in current implementation.
        var handler = new CreateAdminHandler(users);

        var command = new CreateAdminCommand
        {
            FirstName = "Alice",
            LastName = "Smith",
            Email = "alice.admin@example.com",
            Password = "SuperSecret123"
        };

        var result = await handler.Handle(command, System.Threading.CancellationToken.None);

        result.IsSuccess.Should().BeTrue();
        result.Value.Should().NotBeNull();
        result.Value.Email.Should().Be("alice.admin@example.com");
        result.Value.Role.Should().Be("admin");

        var created = await db.Users.FirstOrDefaultAsync(u => u.Email == "alice.admin@example.com");
        created.Should().NotBeNull();
        created!.Role.Should().Be("admin");
        created.Password.Should().NotBe("SuperSecret123");
    }
}
