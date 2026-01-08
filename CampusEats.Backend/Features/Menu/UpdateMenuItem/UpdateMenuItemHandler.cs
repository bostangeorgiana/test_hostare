using CampusEats.Persistence.Context;
using CampusEats.Persistence.Entities;
using CampusEats.Shared.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CampusEats.Features.Menu.UpdateMenuItem;

public class UpdateMenuItemHandler(CampusEatsDbContext context) : IRequestHandler<UpdateMenuItemCommand, Unit>
{
    public async Task<Unit> Handle(UpdateMenuItemCommand request, CancellationToken cancellationToken)
    {
        var item = await context.MenuItems
            .Include(mi => mi.MenuItemLabels)
            .Include(mi => mi.MenuItemIngredients)
            .ThenInclude(mi => mi.Ingredient)
            .FirstOrDefaultAsync(mi => mi.MenuItemId == request.MenuItemId, cancellationToken);

        if (item == null)
            throw new NotFoundException($"Menu item {request.MenuItemId} not found.");
        
        item.Name = request.Name;
        item.Description = request.Description;
        item.PictureLink = request.PictureLink;
        item.Price = request.Price;
        item.Calories = request.Calories;
        item.CurrentStock = request.CurrentStock;
        item.IsAvailable = request.CurrentStock > 0;
        
        item.MenuItemLabels.Clear();
        foreach (var lblId in request.LabelIds)
        {
            item.MenuItemLabels.Add(new MenuItemLabel
            {
                MenuItemId = item.MenuItemId,
                LabelId = lblId
            });
        }
        
        item.MenuItemIngredients.Clear();
        foreach (var ing in request.Ingredients)
        {
            item.MenuItemIngredients.Add(new MenuItemIngredient
            {
                MenuItemId = item.MenuItemId,
                IngredientId = ing.IngredientId,
                Quantity = ing.Quantity
            });
        }

        await context.SaveChangesAsync(cancellationToken);
        return Unit.Value;
    }
}
