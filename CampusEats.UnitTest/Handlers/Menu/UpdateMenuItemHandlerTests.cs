namespace CampusEats.UnitTest.Handlers.Menu;

using System.Threading;
using Xunit;
using System.Collections.Generic;
using CampusEats.Persistence.Entities;
using FluentAssertions;
using CampusEats.UnitTest.Helpers;
using CampusEats.Features.Menu.UpdateMenuItem;
using CampusEats.Features.Menu.CreateMenuItem;
using CampusEats.Shared.Exceptions;


public class UpdateMenuItemHandlerTests
{
    [Fact]
    public async Task Given_ExistingItem_When_Update_Then_FieldsAreUpdated()
    {
        using var dbContext = ContextHelper.CreateInMemoryDbContext();
        var handler = new UpdateMenuItemHandler(dbContext);

        var menuItemId = 1;

        dbContext.MenuItems.Add(new MenuItem 
        { 
            MenuItemId = menuItemId, 
            Name = "Old Name", 
            Price = 10m,
            Description = "Old desc",
            CurrentStock = 5,
            Calories = 500,
            IsAvailable = true
        });
        await dbContext.SaveChangesAsync();

        var command = new UpdateMenuItemCommand
        {
            MenuItemId = menuItemId,
            Name = "Updated Name",
            Description = "Updated desc",
            Price = 20m,
            Calories = 600,
            IsAvailable = true,
            CurrentStock = 10,
            LabelIds = new List<int>(),
            Ingredients = new List<MenuIngredientDto>()
        };

        await handler.Handle(command, CancellationToken.None);

        var updatedItem = await dbContext.MenuItems.FindAsync(menuItemId);
        updatedItem.Should().NotBeNull();
        updatedItem!.Name.Should().Be("Updated Name");
        updatedItem.Price.Should().Be(20m);
        updatedItem.CurrentStock.Should().Be(10);
    }

    [Fact]
    public async Task Given_NonExistentItem_When_Update_Then_ThrowsException()
    {
        using var dbContext = ContextHelper.CreateInMemoryDbContext();
        var handler = new UpdateMenuItemHandler(dbContext);

        var command = new UpdateMenuItemCommand
        {
            MenuItemId = 999,
            Name = "Name",
            Description = "Desc",
            Price = 10,
            Calories = 100,
            IsAvailable = true,
            CurrentStock = 10,
            LabelIds = new List<int>(),
            Ingredients = new List<MenuIngredientDto>()
        };


        await Assert.ThrowsAsync<NotFoundException>(() => handler.Handle(command, CancellationToken.None));
    }
}