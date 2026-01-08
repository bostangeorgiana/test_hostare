namespace CampusEats.UnitTest.Validators.Payment.Process;

using Xunit;
using FluentAssertions;
using CampusEats.Features.Payment.Process;

public class ProcessPaymentValidatorTests
{
    [Fact]
    public async System.Threading.Tasks.Task Given_ValidCommand_When_Validate_Then_Succeeds()
    {
        var validator = new ProcessPaymentValidator();
        var command = new ProcessPaymentCommand(123, "pm_abc", 0);

        var result = await validator.ValidateAsync(command);

        result.IsValid.Should().BeTrue();
    }

    [Fact]
    public async System.Threading.Tasks.Task Given_InvalidOrderId_When_Validate_Then_Fails()
    {
        var validator = new ProcessPaymentValidator();
        var command = new ProcessPaymentCommand(0, "pm_abc", 0);

        var result = await validator.ValidateAsync(command);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "OrderId");
    }

    [Fact]
    public async System.Threading.Tasks.Task Given_EmptyPaymentMethod_When_Validate_Then_Fails()
    {
        var validator = new ProcessPaymentValidator();
        var command = new ProcessPaymentCommand(1, "", 0);

        var result = await validator.ValidateAsync(command);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "PaymentMethodId");
    }

    [Fact]
    public async System.Threading.Tasks.Task Given_NegativePoints_When_Validate_Then_Fails()
    {
        var validator = new ProcessPaymentValidator();
        var command = new ProcessPaymentCommand(1, "pm_abc", -5);

        var result = await validator.ValidateAsync(command);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "PointsToRedeem");
    }
}

