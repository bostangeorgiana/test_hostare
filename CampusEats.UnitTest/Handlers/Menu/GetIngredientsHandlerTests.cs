namespace CampusEats.UnitTest.Handlers.Menu;

using System.Threading;
using System.Collections.Generic;
using Xunit;
using FluentAssertions;
using CampusEats.UnitTest.Helpers;
using CampusEats.Persistence.Entities;
using CampusEats.Features.Menu.GetIngredients;

public class GetIngredientsHandlerTests
{
    [Fact]
    public async Task Given_IngredientsExist_When_Handle_Then_ReturnsOrderedList()
    {
        using var dbContext = ContextHelper.CreateInMemoryDbContext();

        dbContext.Ingredients.AddRange(new List<Ingredient>
        {
            new Ingredient { IngredientId = 1, Name = "Tomato", Allergens = null, CaloriesPerUnit = 20 },
            new Ingredient { IngredientId = 2, Name = "Apple", Allergens = null, CaloriesPerUnit = 30 },
            new Ingredient { IngredientId = 3, Name = "Banana", Allergens = "none", CaloriesPerUnit = 25 }
        });

        await dbContext.SaveChangesAsync();

        var handler = new GetIngredientsHandler(dbContext);
        var query = new GetIngredientsQuery();
        
        var result = await handler.Handle(query, CancellationToken.None);
        
        result.Should().HaveCount(3);
        result[0].Name.Should().Be("Apple");
        result[1].Name.Should().Be("Banana");
        result[2].Name.Should().Be("Tomato");

        result.Should().ContainSingle(r => r.IngredientId == 2 && r.Name == "Apple");
    }

    [Fact]
    public async Task Given_NoIngredients_When_Handle_Then_ReturnsEmptyList()
    {
        using var dbContext = ContextHelper.CreateInMemoryDbContext();
        var handler = new GetIngredientsHandler(dbContext);
        var query = new GetIngredientsQuery();
        
        var result = await handler.Handle(query, CancellationToken.None);
        
        result.Should().NotBeNull();
        result.Should().BeEmpty();
    }
}