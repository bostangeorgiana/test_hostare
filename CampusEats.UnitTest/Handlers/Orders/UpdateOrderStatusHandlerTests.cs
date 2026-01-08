namespace CampusEats.UnitTest.Handlers.Orders;

using FluentAssertions;
using Xunit;
using CampusEats.Persistence.Entities;
using CampusEats.Persistence.Repositories;
using CampusEats.Features.Orders.UpdateOrderStatus;
using CampusEats.UnitTest.Helpers;

public class UpdateOrderStatusHandlerTests
{
    [Fact]
    public async Task Given_ValidTransition_When_UpdateStatus_Then_Success()
    {
        using var dbContext = ContextHelper.CreateInMemoryDbContext();
        var orderRepo = new OrderRepository(dbContext);
        var handler = new UpdateOrderStatusHandler(orderRepo);

        var staffId = 50;
        var orderId = 101;

        dbContext.Users.Add(new User 
        { 
            UserId = staffId, 
            Role = "kitchen_staff", 
            FirstName = "Chef", 
            LastName = "Gordon", 
            Email = "chef@test.com", 
            Password = "dummy_password" 
        });

        dbContext.Orders.Add(new Order
        {
            OrderId = orderId, 
            StudentId = 1, 
            TotalAmount = 20m, 
            Status = "paid",
            CreatedAt = DateTime.UtcNow
        });
        await dbContext.SaveChangesAsync();

        var result = await orderRepo.UpdateOrderStatusAsync(orderId, "preparing", staffId, CancellationToken.None);

        result.Should().BeTrue();
        var updatedOrder = await dbContext.Orders.FindAsync(orderId);
        updatedOrder!.Status.Should().Be("preparing");
    }
    

    [Fact]
    public async Task Given_InvalidRole_When_UpdateStatus_Then_ThrowsException()
    {
        using var dbContext = ContextHelper.CreateInMemoryDbContext();
        var orderRepo = new OrderRepository(dbContext);
        var handler = new UpdateOrderStatusHandler(orderRepo);

        var studentId = 99;
        var orderId = 102;

        dbContext.Users.Add(new User
        {
            UserId = studentId,
            Role = "student",
            FirstName = "S",
            LastName = "T",
            Email = "s@t.com",
            Password = "dummy_password"
        });

        dbContext.Orders.Add(new Order
        {
            OrderId = orderId,
            StudentId = studentId,
            Status = "pending",
            TotalAmount = 10
        });
        await dbContext.SaveChangesAsync();

        var command = new UpdateOrderStatusCommand(orderId, "preparing", studentId);
        await Assert.ThrowsAsync<Exception>(() => handler.Handle(command, CancellationToken.None));
    }

    [Fact]
    public async Task Given_InvalidTransition_When_UpdateStatus_Then_ThrowsException()
    {
        using var dbContext = ContextHelper.CreateInMemoryDbContext();
        var orderRepo = new OrderRepository(dbContext);
        var handler = new UpdateOrderStatusHandler(orderRepo);

        var staffId = 50;
        var orderId = 300;

        dbContext.Users.Add(new User
        {
            UserId = staffId,
            Role = "kitchen_staff",
            FirstName = "Chef",
            LastName = "G",
            Email = "chef@test.com",
            Password = "x"
        });

        dbContext.Orders.Add(new Order
        {
            OrderId = orderId,
            StudentId = 1,
            Status = "pending",
            TotalAmount = 20
        });
        await dbContext.SaveChangesAsync();

        var command = new UpdateOrderStatusCommand(orderId, "completed", staffId);
        await Assert.ThrowsAsync<Exception>(() => handler.Handle(command, CancellationToken.None));
    }

    [Fact]
    public async Task Given_NonExistentOrder_When_UpdateStatus_Then_ThrowsException()
    {
        using var dbContext = ContextHelper.CreateInMemoryDbContext();
        var orderRepo = new OrderRepository(dbContext);
        var handler = new UpdateOrderStatusHandler(orderRepo);

        var staffId = 50;
        var unknownOrderId = 9999;

        dbContext.Users.Add(new User
        {
            UserId = staffId,
            Role = "kitchen_staff",
            FirstName = "Chef",
            LastName = "G",
            Email = "chef@test.com",
            Password = "x"
        });
        await dbContext.SaveChangesAsync();

        var command = new UpdateOrderStatusCommand(unknownOrderId, "preparing", staffId);
        await Assert.ThrowsAsync<Exception>(() => handler.Handle(command, CancellationToken.None));
    }
}
