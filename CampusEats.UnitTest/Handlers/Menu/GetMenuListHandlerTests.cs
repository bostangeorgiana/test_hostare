namespace CampusEats.UnitTest.Handlers.Menu;

using System.Collections.Generic;
using System.Threading.Tasks;
using Moq;
using Xunit;
using FluentAssertions;
using CampusEats.Features.Menu.GetMenuList;
using CampusEats.Features.Menu;
using CampusEats.Features.Menu.Interfaces;

public class GetMenuListHandlerTests
{
    [Fact]
    public async Task Given_ItemsExist_When_Handle_Then_ReturnsList()
    {
        var items = new List<MenuItemDto>
        {
            new MenuItemDto(1, "Pizza", "Desc", null, 10m, 500, true, 5, new List<string>(), new List<MenuIngredientDetailDto>(), false),
            new MenuItemDto(2, "Burger", "Desc", null, 8m, 400, true, 3, new List<string>(), new List<MenuIngredientDetailDto>(), false)
        };

        var repoMock = new Mock<IMenuRepository>();
        repoMock.Setup(r => r.GetMenuListAsync(It.IsAny<List<int>>(), It.IsAny<AvailabilityFilter>(), It.IsAny<bool>(), It.IsAny<decimal?>(), It.IsAny<decimal?>()))
                .ReturnsAsync(items);

        var handler = new GetMenuListHandler(repoMock.Object);
        var query = new GetMenuListQuery();

        var result = await handler.Handle(query, System.Threading.CancellationToken.None);

        result.Should().NotBeNull();
        result.Should().HaveCount(2);
        result[0].Name.Should().Be("Pizza");
    }

    [Fact]
    public async Task Given_NoItems_When_Handle_Then_ReturnsEmptyList()
    {
        var repoMock = new Mock<IMenuRepository>();
        repoMock.Setup(r => r.GetMenuListAsync(It.IsAny<List<int>>(), It.IsAny<AvailabilityFilter>(), It.IsAny<bool>(), It.IsAny<decimal?>(), It.IsAny<decimal?>()))
                .ReturnsAsync(new List<MenuItemDto>());

        var handler = new GetMenuListHandler(repoMock.Object);
        var query = new GetMenuListQuery();

        var result = await handler.Handle(query, System.Threading.CancellationToken.None);

        result.Should().BeEmpty();
    }
}