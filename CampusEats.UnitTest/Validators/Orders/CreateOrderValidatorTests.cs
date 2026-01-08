namespace CampusEats.UnitTest.Validators.Orders.CreateOrder;

using Xunit;
using FluentAssertions;
using CampusEats.Features.Orders.CreateOrder;
using System.Collections.Generic;

public class CreateOrderValidatorTests
{
    [Fact]
    public async System.Threading.Tasks.Task Given_ValidCommand_When_Validate_Then_Succeeds()
    {
        var validator = new CreateOrderValidator();
        var command = new CreateOrderCommand(
            1,
            new List<OrderItemDto>
            {
                new OrderItemDto(1, 2),
                new OrderItemDto(2, 1)
            },
            null // Added Notes parameter
        );

        var result = await validator.ValidateAsync(command);

        result.IsValid.Should().BeTrue();
    }

    [Fact]
    public async System.Threading.Tasks.Task Given_InvalidStudentId_When_Validate_Then_Fails()
    {
        var validator = new CreateOrderValidator();
        var command = new CreateOrderCommand(
            0,
            new List<OrderItemDto>
            {
                new OrderItemDto(1, 2)
            },
            null 
        );

        var result = await validator.ValidateAsync(command);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "StudentId");
    }

    [Fact]
    public async System.Threading.Tasks.Task Given_EmptyItems_When_Validate_Then_Fails()
    {
        var validator = new CreateOrderValidator();
        var command = new CreateOrderCommand(1, new List<OrderItemDto>(), null);

        var result = await validator.ValidateAsync(command);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "Items");
    }

    [Fact]
    public async System.Threading.Tasks.Task Given_InvalidItem_When_Validate_Then_Fails()
    {
        var validator = new CreateOrderValidator();
        var command = new CreateOrderCommand(
            1,
            new List<OrderItemDto>
            {
                new OrderItemDto(0, 1),
                new OrderItemDto(2, 0)
            },
            null 
        );

        var result = await validator.ValidateAsync(command);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName.Contains("MenuItemId") || e.PropertyName.Contains("Quantity"));
    }
}
