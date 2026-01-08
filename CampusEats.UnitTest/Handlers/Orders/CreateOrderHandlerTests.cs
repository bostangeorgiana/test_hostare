namespace CampusEats.UnitTest.Handlers.Orders;

using FluentAssertions;
using Moq;
using Xunit;
using CampusEats.Features.Orders.CreateOrder;
using CampusEats.Features.Menu.Interfaces;
using CampusEats.Exceptions;
using CampusEats.Persistence.Entities;
using CampusEats.Persistence.Repositories;
using CampusEats.UnitTest.Helpers;
using CreateOrderDto = CampusEats.Features.Orders.CreateOrder.OrderItemDto;

public class CreateOrderHandlerTests
{
    private readonly Mock<IMenuRepository> _menuRepoMock;
    private readonly Mock<IUnitOfWork> _uowMock;

    public CreateOrderHandlerTests()
    {
        _menuRepoMock = new Mock<IMenuRepository>();
        _uowMock = new Mock<IUnitOfWork>();
        _uowMock.Setup(u => u.SaveChangesAsync()).ReturnsAsync(0);
    }

    [Fact]
    public async Task Given_ValidRequest_When_Handle_Then_OrderIsCreated()
    {
        using var dbContext = ContextHelper.CreateInMemoryDbContext();
        var orderRepo = new OrderRepository(dbContext);

        var menuItemId = 1;
        var mockMenuItem = new MenuItem
        {
            MenuItemId = menuItemId,
            Name = "Pizza",
            Price = 15.50m,
            CurrentStock = 10
        };

        _menuRepoMock.Setup(repo => repo.GetMenuItemsByIdsAsync(It.IsAny<List<int>>(), It.IsAny<CancellationToken>()))
                     .ReturnsAsync(new Dictionary<int, MenuItem> { { menuItemId, mockMenuItem } });

        var handler = new CreateOrderHandler(orderRepo, _menuRepoMock.Object, _uowMock.Object);

        var items = new List<CreateOrderDto> { new CreateOrderDto(menuItemId, 2) };
        var command = new CreateOrderCommand(100, items, null);

        var orderId = await handler.Handle(command, CancellationToken.None);

        orderId.Should().BeGreaterThan(0);
        var savedOrder = await dbContext.Orders.FindAsync(orderId);
        savedOrder.Should().NotBeNull();
        savedOrder!.TotalAmount.Should().Be(31.00m);
    }

    [Fact]
    public async Task Given_InsufficientStock_When_Handle_Then_ThrowsDomainException()
    {
        using var dbContext = ContextHelper.CreateInMemoryDbContext();
        var orderRepo = new OrderRepository(dbContext);

        var mockMenuItem = new MenuItem
        {
            MenuItemId = 1,
            Name = "Burger",
            Price = 10m,
            CurrentStock = 1
        };

        _menuRepoMock.Setup(repo => repo.GetMenuItemsByIdsAsync(It.IsAny<List<int>>(), It.IsAny<CancellationToken>()))
                     .ReturnsAsync(new Dictionary<int, MenuItem> { { 1, mockMenuItem } });

        var handler = new CreateOrderHandler(orderRepo, _menuRepoMock.Object, _uowMock.Object);

        var items = new List<CreateOrderDto> { new CreateOrderDto(1, 5) };
        var command = new CreateOrderCommand(1, items, null);

        await Assert.ThrowsAsync<DomainException>(() => handler.Handle(command, CancellationToken.None));
    }

    [Fact]
    public async Task Given_NegativeQuantity_When_Handle_Then_ThrowsDomainException()
    {
        using var dbContext = ContextHelper.CreateInMemoryDbContext();
        var orderRepo = new OrderRepository(dbContext);

        var mockMenuItem = new MenuItem
        {
            MenuItemId = 1,
            Name = "Pizza",
            Price = 15.50m,
            CurrentStock = 10
        };

        _menuRepoMock.Setup(repo => repo.GetMenuItemsByIdsAsync(It.IsAny<List<int>>(), It.IsAny<CancellationToken>()))
                     .ReturnsAsync(new Dictionary<int, MenuItem> { { 1, mockMenuItem } });

        var handler = new CreateOrderHandler(orderRepo, _menuRepoMock.Object, _uowMock.Object);

        var items = new List<CreateOrderDto> { new CreateOrderDto(1, -5) };
        var command = new CreateOrderCommand(1, items, null);

        await Assert.ThrowsAsync<DomainException>(() => handler.Handle(command, CancellationToken.None));
    }

    [Fact]
    public async Task Given_UnknownMenuItem_When_Handle_Then_ThrowsDomainException()
    {
        using var dbContext = ContextHelper.CreateInMemoryDbContext();
        var orderRepo = new OrderRepository(dbContext);

        _menuRepoMock.Setup(repo => repo.GetMenuItemsByIdsAsync(It.IsAny<List<int>>(), It.IsAny<CancellationToken>()))
                     .ReturnsAsync(new Dictionary<int, MenuItem>());

        var handler = new CreateOrderHandler(orderRepo, _menuRepoMock.Object, _uowMock.Object);

        var items = new List<CreateOrderDto> { new CreateOrderDto(999, 1) };
        var command = new CreateOrderCommand(1, items, null);

        await Assert.ThrowsAsync<DomainException>(() => handler.Handle(command, CancellationToken.None));
    }

    [Fact]
    public async Task Given_EmptyItemList_When_Handle_Then_ThrowsDomainException()
    {
        using var dbContext = ContextHelper.CreateInMemoryDbContext();
        var orderRepo = new OrderRepository(dbContext);

        var handler = new CreateOrderHandler(orderRepo, _menuRepoMock.Object, _uowMock.Object);

        var command = new CreateOrderCommand(1, new List<CreateOrderDto>(), null);

        await Assert.ThrowsAsync<DomainException>(() => handler.Handle(command, CancellationToken.None));
    }
}
