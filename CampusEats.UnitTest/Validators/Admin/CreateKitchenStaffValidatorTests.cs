namespace CampusEats.UnitTest.Validators.Admin.ManageKitchenStaff;

using Xunit;
using FluentAssertions;
using CampusEats.Features.Admin.ManageKitchenStaff;

public class CreateKitchenStaffValidatorTests
{
    [Fact]
    public async System.Threading.Tasks.Task Given_ValidCommand_When_Validate_Then_Succeeds()
    {
        var validator = new CreateKitchenStaffValidator();
        var command = new CreateKitchenStaffCommand
        {
            FirstName = "Anna",
            LastName = "Chef",
            Email = "anna.chef@example.com",
            Password = "StrongPass123"
        };

        var result = await validator.ValidateAsync(command);

        result.IsValid.Should().BeTrue();
    }

    [Fact]
    public async System.Threading.Tasks.Task Given_InvalidCommand_When_Validate_Then_HasErrors()
    {
        var validator = new CreateKitchenStaffValidator();
        var command = new CreateKitchenStaffCommand
        {
            FirstName = "",
            LastName = "",
            Email = "not-an-email",
            Password = "123"
        };

        var result = await validator.ValidateAsync(command);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "FirstName");
        result.Errors.Should().Contain(e => e.PropertyName == "LastName");
        result.Errors.Should().Contain(e => e.PropertyName == "Email");
        result.Errors.Should().Contain(e => e.PropertyName == "Password");
    }
}

