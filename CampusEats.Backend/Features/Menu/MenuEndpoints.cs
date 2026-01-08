using CampusEats.Features.Menu.CreateMenuItem;
using CampusEats.Features.Menu.DeleteMenuItem;
using CampusEats.Features.Menu.Favorites;
using CampusEats.Features.Menu.GetIngredients;
using CampusEats.Features.Menu.GetMenuItemById;
using CampusEats.Features.Menu.GetMenuLabels;
using CampusEats.Features.Menu.GetMenuList;
using CampusEats.Features.Menu.UpdateMenuItem;
using CampusEats.Features.Menu.UpdateStock;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace CampusEats.Features.Menu;

public static class MenuEndpoints
{
    public static void MapMenuEndpoints(this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("/api/menu");

        // GET /api/menu
        group.MapGet("/", async (
            IMediator mediator,
            [FromQuery] string[]? labels,
            [FromQuery] bool? onlyAvailable,
            [FromQuery] bool? onlyFavorites,
            [FromQuery] decimal? minPrice,
            [FromQuery] decimal? maxPrice
        ) =>
        {
            var query = new GetMenuListQuery
            {
                Labels = labels?.Select(int.Parse).ToList(),
                Availability = onlyAvailable switch
                {
                    true => AvailabilityFilter.Available,
                    _    => AvailabilityFilter.All
                },
                OnlyFavorites = onlyFavorites ?? false,
                MinPrice = minPrice,
                MaxPrice = maxPrice
            };

            var result = await mediator.Send(query);
            return Results.Ok(result);
        });

        // GET /api/menu/{id}
        group.MapGet("/{id:int}", async (int id, IMediator mediator) =>
        {
            var result = await mediator.Send(new GetMenuItemByIdQuery(id));
            return Results.Ok(result);
        });

        // POST /api/menu
        group.MapPost("/", async ([FromBody] CreateMenuItemCommand cmd, [FromServices] IMediator mediator) =>
        {
            var createdId = await mediator.Send(cmd);
            return Results.Ok(createdId);
        });

        // PUT /api/menu/{id}
        group.MapPut("/{id:int}", async (int id, [FromBody] UpdateMenuItemCommand cmd, IMediator mediator) =>
        {
            if (id != cmd.MenuItemId)
                return Results.BadRequest("ID mismatch");

            await mediator.Send(cmd);
            return Results.Ok();
        });

        // DELETE /api/menu/{id}
        group.MapDelete("/{id:int}", async (int id, IMediator mediator) =>
        {
            await mediator.Send(new DeleteMenuItemCommand(id));
            return Results.Ok();
        });

        // PATCH /api/menu/{id}/stock
        group.MapPatch("/{id:int}/stock", async (int id, [FromQuery] int newStock, [FromServices] IMediator mediator) =>
        {
            await mediator.Send(new UpdateStockCommand(id, newStock));
            return Results.Ok();
        });

        // POST /api/menu/favorite
        group.MapPost("/favorite", async ([FromBody] ToggleFavoriteCommand cmd, [FromServices] IMediator mediator) =>
        {
            await mediator.Send(cmd);
            return Results.Ok();
        });
        
        // GET /api/menu/labels
        group.MapGet("/labels", async (IMediator mediator) =>
        {
            var labels = await mediator.Send(new GetMenuLabelsQuery());
            return Results.Ok(labels);
        });

        // GET /api/menu/ingredients
        group.MapGet("/ingredients", async (IMediator mediator) =>
        {
            var ingredients = await mediator.Send(new GetIngredientsQuery());
            return Results.Ok(ingredients);
        });
        
        // GET /api/menu/favorites/{studentId}
        group.MapGet("/favorites/{studentId:int}", async (int studentId, IMediator mediator) =>
        {
            var favorites = await mediator.Send(new GetFavoriteMenuItemsByStudentCommand(studentId));
            return Results.Ok(favorites);
        });



    }
}
