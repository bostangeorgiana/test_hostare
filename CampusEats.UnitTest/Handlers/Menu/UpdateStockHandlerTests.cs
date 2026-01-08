namespace CampusEats.UnitTest.Handlers.Menu;

using Moq;
using Xunit;
using System.Threading;
using System.Collections.Generic;
using System.Threading.Tasks;
using CampusEats.Features.Menu.Interfaces;
using CampusEats.Features.Menu.UpdateStock;

public class UpdateStockHandlerTests
{
    private readonly Mock<IMenuRepository> _menuRepoMock;
    private readonly Mock<IFavoritesRepository> _favRepoMock;
    private readonly Mock<INotificationService> _notifyMock;

    public UpdateStockHandlerTests()
    {
        _menuRepoMock = new Mock<IMenuRepository>();
        _favRepoMock = new Mock<IFavoritesRepository>();
        _notifyMock = new Mock<INotificationService>();
    }

    [Fact]
    public async Task Given_StockZero_When_Update_Then_NotificationsSent()
    {
        var handler = new UpdateStockHandler(_menuRepoMock.Object, _favRepoMock.Object, _notifyMock.Object);
        var itemId = 1;
        var newStock = 0;

        var studentsToNotify = new List<int> { 101, 102 };

        _favRepoMock.Setup(x => x.GetStudentsWhoFavoritedAsync(itemId))
            .ReturnsAsync(studentsToNotify);

        var command = new UpdateStockCommand(itemId, newStock);

        await handler.Handle(command, CancellationToken.None);

        _menuRepoMock.Verify(x => x.UpdateStockAsync(itemId, newStock), Times.Once);

        _notifyMock.Verify(x => x.NotifyAsync(studentsToNotify, It.IsAny<string>()), Times.Once);
    }

    [Fact]
    public async Task Given_StockPositive_When_Update_Then_NoNotificationsSent()
    {
        var handler = new UpdateStockHandler(_menuRepoMock.Object, _favRepoMock.Object, _notifyMock.Object);
        var command = new UpdateStockCommand(1, 10); // Stock is 10 (not 0)

        await handler.Handle(command, CancellationToken.None);
        _menuRepoMock.Verify(x => x.UpdateStockAsync(1, 10), Times.Once);
        _notifyMock.Verify(x => x.NotifyAsync(It.IsAny<List<int>>(), It.IsAny<string>()), Times.Never);
    }
}