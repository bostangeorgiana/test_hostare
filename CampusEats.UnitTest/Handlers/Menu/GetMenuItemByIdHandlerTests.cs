namespace CampusEats.UnitTest.Handlers.Menu;

using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Moq;
using Xunit;
using FluentAssertions;
using CampusEats.Features.Menu.GetMenuItemById;
using CampusEats.Features.Menu.Interfaces;
using CampusEats.Persistence.Entities;

public class GetMenuItemByIdHandlerTests
{
    [Fact]
    public async Task Given_ExistingMenuItem_When_Handle_Then_ReturnsDto()
    {
        var menuRepoMock = new Mock<IMenuRepository>();
        var favoritesRepoMock = new Mock<IFavoritesRepository>();
        var httpContextAccessorMock = new Mock<IHttpContextAccessor>();

        var menuItemId = 10;

        var ingredient = new Ingredient { IngredientId = 1, Name = "Tomato", Allergens = null, CaloriesPerUnit = 5 };
        var menuItem = new MenuItem
        {
            MenuItemId = menuItemId,
            Name = "Test Pizza",
            Description = "Desc",
            Price = 9.5m,
            Calories = 300,
            CurrentStock = 5,
            IsAvailable = true
        };

        menuItem.MenuItemLabels.Add(new MenuItemLabel { MenuItemId = menuItemId, LabelId = 2, Label = new MenuLabel { LabelId = 2, Name = "Spicy" } });
        menuItem.MenuItemIngredients.Add(new MenuItemIngredient { MenuItemId = menuItemId, IngredientId = 1, Ingredient = ingredient, Quantity = 2m });

        menuRepoMock.Setup(r => r.GetMenuItemsByIdsAsync(It.Is<List<int>>(l => l.Contains(menuItemId)), It.IsAny<System.Threading.CancellationToken>()))
                    .ReturnsAsync(new Dictionary<int, MenuItem> { { menuItemId, menuItem } });

        var studentId = 123;
        var claims = new ClaimsPrincipal(new ClaimsIdentity(new[] { new Claim("nameid", studentId.ToString()) }));
        var context = new DefaultHttpContext { User = claims };
        httpContextAccessorMock.SetupGet(h => h.HttpContext).Returns(context);

        favoritesRepoMock.Setup(f => f.ExistsAsync(studentId, menuItemId)).ReturnsAsync(true);

        var handler = new GetMenuItemByIdHandler(menuRepoMock.Object, favoritesRepoMock.Object, httpContextAccessorMock.Object);
        var query = new GetMenuItemByIdQuery(menuItemId);

        var result = await handler.Handle(query, System.Threading.CancellationToken.None);

        result.Should().NotBeNull();
        result.MenuItemId.Should().Be(menuItemId);
        result.Name.Should().Be(menuItem.Name);
        result.Labels.Should().Contain("2");
        result.Ingredients.Should().ContainSingle(i => i.IngredientId == 1 && i.Name == "Tomato");
        result.IsFavorite.Should().BeTrue();
    }

    [Fact]
    public async Task Given_NonExistentMenuItem_When_Handle_Then_ThrowsNotFoundException()
    {
        var menuRepoMock = new Mock<IMenuRepository>();
        var favoritesRepoMock = new Mock<IFavoritesRepository>();
        var httpContextAccessorMock = new Mock<IHttpContextAccessor>();

        var menuItemId = 999;

        menuRepoMock.Setup(r => r.GetMenuItemsByIdsAsync(It.IsAny<List<int>>(), It.IsAny<System.Threading.CancellationToken>()))
                    .ReturnsAsync(new Dictionary<int, MenuItem>());

        var handler = new GetMenuItemByIdHandler(menuRepoMock.Object, favoritesRepoMock.Object, httpContextAccessorMock.Object);
        var query = new GetMenuItemByIdQuery(menuItemId);

        await Assert.ThrowsAsync<CampusEats.Shared.Exceptions.NotFoundException>(() => handler.Handle(query, System.Threading.CancellationToken.None));
    }
}