namespace CampusEats.UnitTest.Handlers.Menu;

using System.Collections.Generic;
using System.Threading;
using CampusEats.Features.Menu.DeleteMenuItem;
using CampusEats.Features.Menu.Interfaces;
using CampusEats.Persistence.Entities;
using CampusEats.Shared.Exceptions;
using Moq;
using Xunit;

public class DeleteMenuItemHandlerTests
{
    private readonly Mock<IMenuRepository> _menuRepoMock;

    public DeleteMenuItemHandlerTests()
    {
        _menuRepoMock = new Mock<IMenuRepository>();
    }

    [Fact]
    public async Task Given_ExistingItem_When_Handle_Then_DeleteMenuItemIsCalled()
    {
        var handler = new DeleteMenuItemHandler(_menuRepoMock.Object);
        var itemId = 5;

        var items = new Dictionary<int, MenuItem>
        {
            { itemId, new MenuItem { MenuItemId = itemId, Name = "ToDelete", CurrentStock = 1, IsAvailable = true } }
        };

        _menuRepoMock.Setup(x => x.GetMenuItemsByIdsAsync(It.Is<List<int>>(l => l.Contains(itemId)), It.IsAny<CancellationToken>()))
            .ReturnsAsync(items);

        _menuRepoMock.Setup(x => x.DeleteMenuItemAsync(itemId)).Returns(Task.CompletedTask);

        var command = new DeleteMenuItemCommand(itemId);
        
        await handler.Handle(command, CancellationToken.None);

        _menuRepoMock.Verify(x => x.DeleteMenuItemAsync(itemId), Times.Once);
    }

    [Fact]
    public async Task Given_NonExistentItem_When_Handle_Then_ThrowsNotFoundException()
    {
       
        var handler = new DeleteMenuItemHandler(_menuRepoMock.Object);
        var nonExistentId = 999;

        _menuRepoMock.Setup(x => x.GetMenuItemsByIdsAsync(It.IsAny<List<int>>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new Dictionary<int, MenuItem>());

        var command = new DeleteMenuItemCommand(nonExistentId);
        
        await Assert.ThrowsAsync<NotFoundException>(() => handler.Handle(command, CancellationToken.None));
    }
}