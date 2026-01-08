namespace CampusEats.UnitTest.Validators.Menu.CreateMenuItem;

using Xunit;
using FluentAssertions;
using CampusEats.Features.Menu.CreateMenuItem;
using System.Collections.Generic;

public class CreateMenuItemValidatorTests
{
    [Fact]
    public async System.Threading.Tasks.Task Given_ValidCommand_When_Validate_Then_Succeeds()
    {
        var validator = new CreateMenuItemValidator();
        var command = new CreateMenuItemCommand(
            "Pizza",
            "Delicious",
            9.99m,
            800,
            10,
            new List<int>(),
            new List<MenuIngredientDto>(),
            null
        );

        var result = await validator.ValidateAsync(command);

        result.IsValid.Should().BeTrue();
    }

    [Fact]
    public async System.Threading.Tasks.Task Given_InvalidCommand_When_Validate_Then_HasErrors()
    {
        var validator = new CreateMenuItemValidator();
        var command = new CreateMenuItemCommand(
            "",
            "",
            0m,
            0,
            -1,
            new List<int>(),
            new List<MenuIngredientDto>(),
            null
        );

        var result = await validator.ValidateAsync(command);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "Name");
        result.Errors.Should().Contain(e => e.PropertyName == "Price");
        result.Errors.Should().Contain(e => e.PropertyName == "Stock");
    }
}
