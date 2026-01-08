namespace CampusEats.UnitTest.Validators.Auth.Register;

using Xunit;
using FluentAssertions;
using CampusEats.Features.Auth.Register;
using CampusEats.UnitTest.Helpers;
using CampusEats.Persistence.Repositories;
using CampusEats.Persistence.Entities;

public class RegisterValidatorTests
{
    [Fact]
    public async System.Threading.Tasks.Task Given_ValidCommand_When_Validate_Then_Succeeds()
    {
        using var db = ContextHelper.CreateInMemoryDbContext();
        var users = new UserRepository(db);
        var validator = new RegisterValidator(users);

        var command = new RegisterCommand("John", "Doe", "john.doe@example.com", "password123", "password123");

        var result = await validator.ValidateAsync(command);

        result.IsValid.Should().BeTrue();
    }

    [Fact]
    public async System.Threading.Tasks.Task Given_EmailAlreadyExists_When_Validate_Then_Fails()
    {
        using var db = ContextHelper.CreateInMemoryDbContext();
        db.Users.Add(new User { UserId = 1, Email = "existing@example.com", FirstName = "E", LastName = "X", Password = "x", Role = "student" });
        await db.SaveChangesAsync();

        var users = new UserRepository(db);
        var validator = new RegisterValidator(users);

        var command = new RegisterCommand("Jane", "Doe", "existing@example.com", "password123", "password123");

        var result = await validator.ValidateAsync(command);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "Email" && e.ErrorMessage.Contains("already registered"));
    }

    [Fact]
    public async System.Threading.Tasks.Task Given_InvalidNamesOrPasswords_When_Validate_Then_HasErrors()
    {
        using var db = ContextHelper.CreateInMemoryDbContext();
        var users = new UserRepository(db);
        var validator = new RegisterValidator(users);

        var command = new RegisterCommand("john", "", "bad", "short", "diff");

        var result = await validator.ValidateAsync(command);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "FirstName");
        result.Errors.Should().Contain(e => e.PropertyName == "LastName");
        result.Errors.Should().Contain(e => e.PropertyName == "Email");
        result.Errors.Should().Contain(e => e.PropertyName == "Password");
        result.Errors.Should().Contain(e => e.PropertyName == "ConfirmPassword");
    }
}

