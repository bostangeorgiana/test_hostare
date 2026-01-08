namespace CampusEats.UnitTest.Handlers.Menu;

using System.Threading;
using CampusEats.Features.Menu.Interfaces;
using CampusEats.Features.Menu.Favorites;
using Moq;
using Xunit;


public class ToggleFavoriteHandlerTests
{
    private readonly Mock<IFavoritesRepository> _favRepoMock;

    public ToggleFavoriteHandlerTests()
    {
        _favRepoMock = new Mock<IFavoritesRepository>();
    }

    [Fact]
    public async Task Given_FavoriteExists_When_Handle_Then_RemovesFavorite()
    {
        var handler = new ToggleFavoriteHandler(_favRepoMock.Object);
        var studentId = 1;
        var menuItemId = 10;

        _favRepoMock.Setup(x => x.ExistsAsync(studentId, menuItemId)).ReturnsAsync(true);

        await handler.Handle(new ToggleFavoriteCommand(studentId, menuItemId), CancellationToken.None);

        _favRepoMock.Verify(x => x.RemoveAsync(studentId, menuItemId), Times.Once);
        _favRepoMock.Verify(x => x.AddAsync(It.IsAny<int>(), It.IsAny<int>()), Times.Never);
    }

    [Fact]
    public async Task Given_FavoriteDoesNotExist_When_Handle_Then_AddsFavorite()
    {
        var handler = new ToggleFavoriteHandler(_favRepoMock.Object);
        var studentId = 1;
        var menuItemId = 10;

        _favRepoMock.Setup(x => x.ExistsAsync(studentId, menuItemId)).ReturnsAsync(false);

        await handler.Handle(new ToggleFavoriteCommand(studentId, menuItemId), CancellationToken.None);

        _favRepoMock.Verify(x => x.AddAsync(studentId, menuItemId), Times.Once);
        _favRepoMock.Verify(x => x.RemoveAsync(It.IsAny<int>(), It.IsAny<int>()), Times.Never);
    }
}