namespace CampusEats.UnitTest.Validators.Auth.Login;

using Xunit;
using FluentAssertions;
using CampusEats.Features.Auth.Login;

public class LoginValidatorTests
{
    [Fact]
    public async System.Threading.Tasks.Task Given_ValidCommand_When_Validate_Then_Succeeds()
    {
        var validator = new LoginUserValidator();
        var command = new LoginCommand("user@example.com", "strongpassword");

        var result = await validator.ValidateAsync(command);

        result.IsValid.Should().BeTrue();
    }

    [Fact]
    public async System.Threading.Tasks.Task Given_InvalidCommand_When_Validate_Then_HasErrors()
    {
        var validator = new LoginUserValidator();
        var command = new LoginCommand("bad", "short");

        var result = await validator.ValidateAsync(command);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "Email");
        result.Errors.Should().Contain(e => e.PropertyName == "Password");
    }
}

