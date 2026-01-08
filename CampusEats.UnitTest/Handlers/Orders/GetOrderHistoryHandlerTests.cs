namespace CampusEats.UnitTest.Handlers.Orders;

using System.Threading;
using System.Collections.Generic;
using Moq;
using Xunit;
using FluentAssertions;
using CampusEats.Features.Orders.GetOrderHistory;
using CampusEats.Features.Orders;

public class GetOrderHistoryHandlerTests
{
    [Fact]
    public async Task Given_HistoryExists_When_Handle_Then_ReturnsList()
    {
        var studentId = 42;
        var list = new List<OrderSummaryDto>
        {
            new OrderSummaryDto(1, 10m, "completed", System.DateTime.UtcNow),
            new OrderSummaryDto(2, 20m, "pending", System.DateTime.UtcNow)
        };

        var repoMock = new Mock<CampusEats.Features.Orders.IOrderRepository>();
        repoMock.Setup(r => r.GetOrderHistoryAsync(studentId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(list);

        var handler = new GetOrderHistoryHandler(repoMock.Object);
        var command = new GetOrderHistoryCommand(studentId);
        
        var result = await handler.Handle(command, CancellationToken.None);
        
        result.Should().NotBeNull();
        result.Should().HaveCount(2);
        result[0].OrderId.Should().Be(1);
    }

    [Fact]
    public async Task Given_NoHistory_When_Handle_Then_ReturnsEmptyList()
    {
        var studentId = 100;
        var repoMock = new Mock<CampusEats.Features.Orders.IOrderRepository>();
        repoMock.Setup(r => r.GetOrderHistoryAsync(studentId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<OrderSummaryDto>());

        var handler = new GetOrderHistoryHandler(repoMock.Object);
        var command = new GetOrderHistoryCommand(studentId);
        
        var result = await handler.Handle(command, CancellationToken.None);
        
        result.Should().NotBeNull();
        result.Should().BeEmpty();
    }
}