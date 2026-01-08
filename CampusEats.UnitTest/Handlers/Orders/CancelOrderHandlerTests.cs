namespace CampusEats.UnitTest.Handlers.Orders;

using CampusEats.UnitTest.Helpers;
using CampusEats.Persistence.Entities;
using CampusEats.Persistence.Repositories;
using FluentAssertions;
using Xunit;

public class CancelOrderHandlerTests
{
    [Fact]
    public async Task Given_PendingOrder_When_Cancel_Then_StatusIsCancelled()
    {
        using var dbContext = ContextHelper.CreateInMemoryDbContext();
        var orderRepo = new OrderRepository(dbContext);

        var studentId = 1;
        var orderId = 200;
        
        dbContext.Orders.Add(new Order
        {
            OrderId = orderId,
            StudentId = studentId,
            Status = "pending",
            TotalAmount = 50m
        });
        await dbContext.SaveChangesAsync();

        var result = await orderRepo.CancelOrderAsync(orderId, studentId, CancellationToken.None);

        result.Should().BeTrue();
        var order = await dbContext.Orders.FindAsync(orderId);
        order!.Status.Should().Be("cancelled");
    }

    [Fact]
    public async Task Given_PreparingOrder_When_Cancel_Then_ThrowsException()
    {
        using var dbContext = ContextHelper.CreateInMemoryDbContext();
        var orderRepo = new OrderRepository(dbContext);

        var studentId = 1;
        var orderId = 201;

        // Create 'Preparing' Order (Cannot be cancelled)
        dbContext.Orders.Add(new Order
        {
            OrderId = orderId,
            StudentId = studentId,
            Status = "preparing", 
            TotalAmount = 50m
        });
        await dbContext.SaveChangesAsync();

        await Assert.ThrowsAsync<Exception>(() => 
            orderRepo.CancelOrderAsync(orderId, studentId, CancellationToken.None));
    }

    [Fact]
    public async Task Given_NonExistentOrder_When_Cancel_Then_ThrowsException()
    {
        using var dbContext = ContextHelper.CreateInMemoryDbContext();
        var orderRepo = new OrderRepository(dbContext);

        var studentId = 1;
        var nonExistentOrderId = 999;

        await Assert.ThrowsAsync<Exception>(() => 
            orderRepo.CancelOrderAsync(nonExistentOrderId, studentId, CancellationToken.None));
    }
    [Fact]
    public async Task Given_OrderBelongingToAnotherStudent_When_Cancel_Then_ThrowsException()
    {
        using var dbContext = ContextHelper.CreateInMemoryDbContext();
        var orderRepo = new OrderRepository(dbContext);

        var ownerStudentId = 1;
        var maliciousStudentId = 2; 
        var orderId = 300;

        dbContext.Orders.Add(new Order
        {
            OrderId = orderId,
            StudentId = ownerStudentId, 
            Status = "pending",
            TotalAmount = 50m
        });
        await dbContext.SaveChangesAsync();

        await Assert.ThrowsAsync<Exception>(() => 
            orderRepo.CancelOrderAsync(orderId, maliciousStudentId, CancellationToken.None));
    }

    [Fact]
    public async Task Given_CompletedOrder_When_Cancel_Then_ThrowsException()
    {
        using var dbContext = ContextHelper.CreateInMemoryDbContext();
        var orderRepo = new OrderRepository(dbContext);

        var studentId = 1;
        var orderId = 400;

        dbContext.Orders.Add(new Order
        {
            OrderId = orderId,
            StudentId = studentId,
            Status = "completed", // Too late to cancel
            TotalAmount = 50m
        });
        await dbContext.SaveChangesAsync();


        await Assert.ThrowsAsync<Exception>(() => 
            orderRepo.CancelOrderAsync(orderId, studentId, CancellationToken.None));
    }
}