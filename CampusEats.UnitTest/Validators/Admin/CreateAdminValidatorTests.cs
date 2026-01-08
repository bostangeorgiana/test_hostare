namespace CampusEats.UnitTest.Validators.Admin.ManageAdmin;

using Xunit;
using FluentAssertions;
using CampusEats.Features.Admin.ManageAdmin.CreateAdmin;

public class CreateAdminValidatorTests
{
    [Fact]
    public async System.Threading.Tasks.Task Given_ValidCommand_When_Validate_Then_Succeeds()
    {
        var validator = new CreateAdminValidator();
        var command = new CreateAdminCommand
        {
            FirstName = "John",
            LastName = "Doe",
            Email = "john.doe@example.com",
            Password = "strongpass"
        };

        var result = await validator.ValidateAsync(command);

        result.IsValid.Should().BeTrue();
    }

    [Fact]
    public async System.Threading.Tasks.Task Given_InvalidCommand_When_Validate_Then_HasErrors()
    {
        var validator = new CreateAdminValidator();
        var command = new CreateAdminCommand
        {
            FirstName = "",
            LastName = "",
            Email = "bademail",
            Password = "short"
        };

        var result = await validator.ValidateAsync(command);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "FirstName");
        result.Errors.Should().Contain(e => e.PropertyName == "LastName");
        result.Errors.Should().Contain(e => e.PropertyName == "Email");
        result.Errors.Should().Contain(e => e.PropertyName == "Password");
    }
}

