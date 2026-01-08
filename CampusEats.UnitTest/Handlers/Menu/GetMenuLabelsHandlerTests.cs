namespace CampusEats.UnitTest.Handlers.Menu;

using System.Threading;
using System.Collections.Generic;
using Moq;
using Xunit;
using FluentAssertions;
using CampusEats.Features.Menu.GetMenuLabels;

public class GetMenuLabelsHandlerTests
{
    [Fact]
    public async Task Given_LabelsExist_When_Handle_Then_ReturnsList()
    {
        var list = new List<GetMenuLabelsResponse>
        {
            new GetMenuLabelsResponse(1, "Vegan"),
            new GetMenuLabelsResponse(2, "Spicy")
        };

        var repoMock = new Mock<CampusEats.Features.Menu.Interfaces.IMenuRepository>();
        repoMock.Setup(r => r.GetMenuLabelsAsync(It.IsAny<CancellationToken>())).ReturnsAsync(list);

        var handler = new GetMenuLabelsHandler(repoMock.Object);
        var query = new GetMenuLabelsQuery();

        var result = await handler.Handle(query, CancellationToken.None);

        result.Should().NotBeNull();
        result.Should().HaveCount(2);
        result[0].Name.Should().Be("Vegan");
    }

    [Fact]
    public async Task Given_NoLabels_When_Handle_Then_ReturnsEmptyList()
    {
        var repoMock = new Mock<CampusEats.Features.Menu.Interfaces.IMenuRepository>();
        repoMock.Setup(r => r.GetMenuLabelsAsync(It.IsAny<CancellationToken>())).ReturnsAsync(new List<GetMenuLabelsResponse>());

        var handler = new GetMenuLabelsHandler(repoMock.Object);
        var query = new GetMenuLabelsQuery();

        var result = await handler.Handle(query, CancellationToken.None);

        result.Should().BeEmpty();
    }
}