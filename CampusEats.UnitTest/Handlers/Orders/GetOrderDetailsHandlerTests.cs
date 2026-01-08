namespace CampusEats.UnitTest.Handlers.Orders;

using System.Threading;
using System.Collections.Generic;
using Moq;
using Xunit;
using FluentAssertions;
using CampusEats.Features.Orders.GetOrderDetails;
using CampusEats.Features.Orders;

public class GetOrderDetailsHandlerTests
{
    [Fact]
    public async Task Given_ExistingOrder_When_Handle_Then_ReturnsDetails()
    {
        var orderId = 123;
        var items = new List<OrderItemDetailsDto>
        {
            new OrderItemDetailsDto(1, "Pizza", 2, 12.5m),
            new OrderItemDetailsDto(2, "Soda", 1, 2.0m)
        };

        var dto = new OrderDetailsDto(
            orderId,
            27.0m,
            "completed",
            System.DateTime.UtcNow,
            items,
            0,
            0,
            27.0m
        );

        var repoMock = new Mock<CampusEats.Features.Orders.IOrderRepository>();
        repoMock.Setup(r => r.GetOrderDetailsAsync(orderId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(dto);

        var handler = new GetOrderDetailsHandler(repoMock.Object);
        var command = new GetOrderDetailsCommand(orderId);
        
        var result = await handler.Handle(command, CancellationToken.None);
        
        result.Should().NotBeNull();
        result!.OrderId.Should().Be(orderId);
        result.Items.Should().HaveCount(2);
        result.TotalAmount.Should().Be(dto.TotalAmount);
    }

    [Fact]
    public async Task Given_NonExistentOrder_When_Handle_Then_ReturnsNull()
    {
        var orderId = 999;
        var repoMock = new Mock<CampusEats.Features.Orders.IOrderRepository>();
        repoMock.Setup(r => r.GetOrderDetailsAsync(orderId, It.IsAny<CancellationToken>()))
                .ReturnsAsync((OrderDetailsDto?)null);

        var handler = new GetOrderDetailsHandler(repoMock.Object);
        var command = new GetOrderDetailsCommand(orderId);
        
        var result = await handler.Handle(command, CancellationToken.None);
        
        result.Should().BeNull();
    }
}