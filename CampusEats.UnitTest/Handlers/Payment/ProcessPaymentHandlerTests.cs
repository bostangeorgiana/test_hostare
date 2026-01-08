namespace CampusEats.UnitTest.Handlers.Payment;

using Xunit;
using Moq;
using FluentAssertions;
using CampusEats.Features.Payment.Process;
using CampusEats.Persistence.Repositories;
using CampusEats.UnitTest.Helpers;
using CampusEats.Persistence.Entities;
using CampusEats.Features.Orders;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;

public class ProcessPaymentHandlerTests
{
    [Fact]
    public async Task Given_OrderNotFound_When_Handle_Then_ReturnsFailure()
    {
        var ordersMock = new Mock<IOrderRepository>();
        ordersMock.Setup(o => o.GetBasicOrderAsync(It.IsAny<int>(), It.IsAny<CancellationToken>())).ReturnsAsync((Order?)null);

        using var db = ContextHelper.CreateInMemoryDbContext();
        var payments = new PaymentRepository(db);
        var students = new StudentRepository(db);

        var menuMock = new Mock<CampusEats.Features.Menu.Interfaces.IMenuRepository>();
        var uowMock = new Mock<CampusEats.Features.Menu.Interfaces.IUnitOfWork>();
        var configMock = new Mock<IConfiguration>();
        var loggerMock = new Mock<ILogger<ProcessPaymentHandler>>();

        var handler = new ProcessPaymentHandler(ordersMock.Object, payments, students, menuMock.Object, uowMock.Object, configMock.Object, loggerMock.Object);

        var result = await handler.Handle(new ProcessPaymentCommand(999, "pm", 0), CancellationToken.None);

        result.IsSuccess.Should().BeFalse();
        result.Error.Should().Contain("Order not found");
    }

    [Fact]
    public async Task Given_OrderNotPending_When_Handle_Then_ReturnsFailure()
    {
        var ordersMock = new Mock<IOrderRepository>();
        var order = new Order { OrderId = 1, Status = "paid", TotalAmount = 10m, StudentId = 1 };
        ordersMock.Setup(o => o.GetBasicOrderAsync(1, It.IsAny<CancellationToken>())).ReturnsAsync(order);

        using var db = ContextHelper.CreateInMemoryDbContext();
        var payments = new PaymentRepository(db);
        var students = new StudentRepository(db);

        var menuMock = new Mock<CampusEats.Features.Menu.Interfaces.IMenuRepository>();
        var uowMock = new Mock<CampusEats.Features.Menu.Interfaces.IUnitOfWork>();
        var configMock = new Mock<IConfiguration>();
        var loggerMock = new Mock<ILogger<ProcessPaymentHandler>>();

        var handler = new ProcessPaymentHandler(ordersMock.Object, payments, students, menuMock.Object, uowMock.Object, configMock.Object, loggerMock.Object);

        var result = await handler.Handle(new ProcessPaymentCommand(1, "pm", 0), CancellationToken.None);

        result.IsSuccess.Should().BeFalse();
        result.Error.Should().Contain("Payment is not allowed");
    }

    [Fact]
    public async Task Given_OrderAlreadyPaid_When_Handle_Then_ReturnsFailure()
    {
        var ordersMock = new Mock<IOrderRepository>();
        var order = new Order { OrderId = 2, Status = "pending", TotalAmount = 20m, StudentId = 1, OrderItems = new List<OrderItem>() };
        ordersMock.Setup(o => o.GetBasicOrderAsync(2, It.IsAny<CancellationToken>())).ReturnsAsync(order);

        using var db = ContextHelper.CreateInMemoryDbContext();
        // Seed a succeeded payment for order 2
        db.Payments.Add(new Payment { PaymentId = 1, OrderId = 2, Status = "succeeded", Amount = 20m, CreatedAt = System.DateTime.UtcNow });
        await db.SaveChangesAsync();

        var payments = new PaymentRepository(db);
        var students = new StudentRepository(db);

        var menuMock = new Mock<CampusEats.Features.Menu.Interfaces.IMenuRepository>();
        var uowMock = new Mock<CampusEats.Features.Menu.Interfaces.IUnitOfWork>();
        var configMock = new Mock<IConfiguration>();
        var loggerMock = new Mock<ILogger<ProcessPaymentHandler>>();

        var handler = new ProcessPaymentHandler(ordersMock.Object, payments, students, menuMock.Object, uowMock.Object, configMock.Object, loggerMock.Object);

        var result = await handler.Handle(new ProcessPaymentCommand(2, "pm", 0), CancellationToken.None);

        result.IsSuccess.Should().BeFalse();
        result.Error.Should().Contain("already fully paid");
    }

    [Fact]
    public async Task Given_OrderAmountInvalid_When_Handle_Then_ReturnsFailure()
    {
        var ordersMock = new Mock<IOrderRepository>();
        var order = new Order { OrderId = 3, Status = "pending", TotalAmount = 0m, StudentId = 1 };
        ordersMock.Setup(o => o.GetBasicOrderAsync(3, It.IsAny<CancellationToken>())).ReturnsAsync(order);

        using var db = ContextHelper.CreateInMemoryDbContext();
        var payments = new PaymentRepository(db);
        var students = new StudentRepository(db);

        var menuMock = new Mock<CampusEats.Features.Menu.Interfaces.IMenuRepository>();
        var uowMock = new Mock<CampusEats.Features.Menu.Interfaces.IUnitOfWork>();
        var configMock = new Mock<IConfiguration>();
        var loggerMock = new Mock<ILogger<ProcessPaymentHandler>>();

        var handler = new ProcessPaymentHandler(ordersMock.Object, payments, students, menuMock.Object, uowMock.Object, configMock.Object, loggerMock.Object);

        var result = await handler.Handle(new ProcessPaymentCommand(3, "pm", 0), CancellationToken.None);

        result.IsSuccess.Should().BeFalse();
        result.Error.Should().Contain("Order amount is invalid");
    }

    [Fact]
    public async Task Given_MissingStripeConfig_When_Handle_Then_ReturnsFailure()
    {
        var ordersMock = new Mock<IOrderRepository>();
        var order = new Order { OrderId = 4, Status = "pending", TotalAmount = 15m, StudentId = 1, OrderItems = new List<OrderItem>() };
        ordersMock.Setup(o => o.GetBasicOrderAsync(4, It.IsAny<CancellationToken>())).ReturnsAsync(order);

        using var db = ContextHelper.CreateInMemoryDbContext();
        var payments = new PaymentRepository(db);
        var students = new StudentRepository(db);

        var menuMock = new Mock<CampusEats.Features.Menu.Interfaces.IMenuRepository>();
        var uowMock = new Mock<CampusEats.Features.Menu.Interfaces.IUnitOfWork>();
        var configMock = new Mock<IConfiguration>();
        configMock.Setup(c => c["Stripe:SecretKey"]).Returns((string?)null);
        var loggerMock = new Mock<ILogger<ProcessPaymentHandler>>();

        var handler = new ProcessPaymentHandler(ordersMock.Object, payments, students, menuMock.Object, uowMock.Object, configMock.Object, loggerMock.Object);

        var result = await handler.Handle(new ProcessPaymentCommand(4, "pm", 0), CancellationToken.None);

        result.IsSuccess.Should().BeFalse();
        result.Error.Should().Contain("Payment configuration error");
    }
}

