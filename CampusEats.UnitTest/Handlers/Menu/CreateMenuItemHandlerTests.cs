namespace CampusEats.UnitTest.Handlers.Menu;

using CampusEats.Features.Menu.Interfaces;
using CampusEats.Features.Menu.CreateMenuItem;
using Moq;
using Xunit;

public class CreateMenuItemHandlerTests
{
    private readonly Mock<IMenuRepository> _menuRepoMock;
    private readonly Mock<IUnitOfWork> _uowMock;

    public CreateMenuItemHandlerTests()
    {
        _menuRepoMock = new Mock<IMenuRepository>();
        _uowMock = new Mock<IUnitOfWork>();
    }

    [Fact]
    public async Task Given_ValidRequest_When_Handle_Then_ItemIsCreated()
    {
        var handler = new CreateMenuItemHandler(_menuRepoMock.Object, _uowMock.Object);
        var expectedId = 10;

        var command = new CreateMenuItemCommand
        (
            "New Pizza",
            "Tasty cheese pizza",
            12.50m,
            800,
            50,
            new List<int> { 1, 2 },
            new List<MenuIngredientDto>(),
            null
        );
        
        _menuRepoMock.Setup(x => x.CreateMenuItemAsync(
                It.IsAny<string>(), It.IsAny<string>(), It.IsAny<decimal>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string?>()))
            .ReturnsAsync(expectedId);

        var result = await handler.Handle(command, CancellationToken.None);

        Assert.Equal(expectedId, result);

        _menuRepoMock.Verify(x => x.AssignLabelsAsync(expectedId, command.LabelIds), Times.Once);
        _menuRepoMock.Verify(x => x.AssignIngredientsAsync(expectedId, command.Ingredients), Times.Once);

        _uowMock.Verify(x => x.SaveChangesAsync(), Times.Once);

    }
}