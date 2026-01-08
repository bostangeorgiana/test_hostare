using CampusEats.Features.Menu;
using CampusEats.Features.Menu.CreateMenuItem;
using CampusEats.Features.Menu.Interfaces;
using CampusEats.Persistence.Context;
using CampusEats.Persistence.Entities;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using CampusEats.Features.Menu.GetMenuLabels;

namespace CampusEats.Persistence.Repositories;

public class MenuRepository(
    CampusEatsDbContext dbContext,
    IFavoritesRepository favoritesRepo,
    IHttpContextAccessor httpContextAccessor)
    : IMenuRepository
{
    public async Task<int> CreateMenuItemAsync(
        string name,
        string description,
        decimal price,
        int calories,
        int stock,
        string? pictureLink)
    {
        var entity = new MenuItem
        {
            Name = name,
            Description = description,
            Price = price,
            Calories = calories,
            CurrentStock = stock,
            IsAvailable = stock > 0,
            PictureLink = pictureLink
        };

        dbContext.MenuItems.Add(entity);
        await dbContext.SaveChangesAsync();

        return entity.MenuItemId;
    }

    public async Task AssignLabelsAsync(int menuItemId, List<int> labelIds)
    {
        var item = await dbContext.MenuItems
            .Include(m => m.MenuItemLabels)
            .FirstOrDefaultAsync(m => m.MenuItemId == menuItemId);

        if (item == null)
            return;

        foreach (int labelId in labelIds)
        {
            if (!item.MenuItemLabels.Any(l => l.LabelId == labelId))
            {
                item.MenuItemLabels.Add(new MenuItemLabel
                {
                    MenuItemId = menuItemId,
                    LabelId = labelId
                });
            }
        }

        await dbContext.SaveChangesAsync();
    }

    public async Task AssignIngredientsAsync(int menuItemId, List<MenuIngredientDto> ingredients)
    {
        var item = await dbContext.MenuItems
            .Include(m => m.MenuItemIngredients)
            .FirstOrDefaultAsync(m => m.MenuItemId == menuItemId);

        if (item == null)
            return;

        foreach (var i in ingredients)
        {
            if (!item.MenuItemIngredients.Any(x => x.IngredientId == i.IngredientId))
            {
                item.MenuItemIngredients.Add(new MenuItemIngredient
                {
                    MenuItemId = menuItemId,
                    IngredientId = i.IngredientId,
                    Quantity = i.Quantity
                });
            }
        }

        await dbContext.SaveChangesAsync();
    }

    public async Task<List<MenuItemDto>> GetMenuListAsync(
        List<int>? labelIds,
        AvailabilityFilter availability,
        bool onlyFavorites,
        decimal? minPrice,
        decimal? maxPrice)
    {
        var studentIdClaim = httpContextAccessor.HttpContext?.User
            .FindFirst(ClaimTypes.NameIdentifier)
            ?.Value;

        int.TryParse(studentIdClaim, out int studentId);

        var favoriteIds = await favoritesRepo.GetFavoriteMenuItemIdsAsync(studentId);

        var query = dbContext.MenuItems
            .Include(m => m.MenuItemLabels).ThenInclude(l => l.Label)
            .Include(m => m.MenuItemIngredients).ThenInclude(i => i.Ingredient)
            .AsQueryable();

        query = availability switch
        {
            AvailabilityFilter.Available => query.Where(m => m.IsAvailable && m.CurrentStock > 0),
            AvailabilityFilter.Unavailable => query.Where(m => !m.IsAvailable),
            _ => query
        };
        
        if (labelIds is { Count: > 0 })
        {
            query = query.Where(m =>
                m.MenuItemLabels.Any(l => labelIds.Contains(l.LabelId)));
        }
        
        if (onlyFavorites)
        {
            query = query.Where(m => favoriteIds.Contains(m.MenuItemId));
        }
        
        if (minPrice.HasValue)
            query = query.Where(m => m.Price >= minPrice.Value);

        if (maxPrice.HasValue)
            query = query.Where(m => m.Price <= maxPrice.Value);

        var items = await query.ToListAsync();

        return items.Select(m => new MenuItemDto(
            m.MenuItemId,
            m.Name,
            m.Description ?? "",
            m.PictureLink,
            m.Price,
            m.Calories,
            m.IsAvailable,
            m.CurrentStock,
            m.MenuItemLabels.Select(l => l.Label.Name).ToList(),
            m.MenuItemIngredients.Select(i => new MenuIngredientDetailDto(
                i.IngredientId,
                i.Ingredient.Name,
                i.Quantity,
                i.Ingredient.Allergens,
                i.Ingredient.CaloriesPerUnit
            )).ToList(),
            favoriteIds.Contains(m.MenuItemId)
        )).ToList();
    }

    public async Task UpdateStockAsync(int menuItemId, int stock)
    {
        var item = await dbContext.MenuItems.FindAsync(menuItemId);
        if (item == null)
            return;

        item.CurrentStock = stock;
        item.IsAvailable = stock > 0;

        await dbContext.SaveChangesAsync();
    }

    public async Task<Dictionary<int, MenuItem>> GetMenuItemsByIdsAsync(
        List<int> ids,
        CancellationToken ct)
    {
        return await dbContext.MenuItems
            .Where(m => ids.Contains(m.MenuItemId))
            .ToDictionaryAsync(m => m.MenuItemId, ct);
    }

    public async Task DecreaseStockAsync(
        int menuItemId,
        int quantity,
        CancellationToken ct)
    {
        var item = await dbContext.MenuItems.FindAsync(new object[] { menuItemId }, ct);

        if (item == null)
            return;

        item.CurrentStock -= quantity;
        item.IsAvailable = item.CurrentStock > 0;

        await dbContext.SaveChangesAsync(ct);
    }

    public async Task DeleteMenuItemAsync(int menuItemId)
    {
        var item = await dbContext.MenuItems.FindAsync(menuItemId);
        if (item == null)
            return;
        
        item.IsAvailable = false;
        item.CurrentStock = 0;

        await dbContext.SaveChangesAsync();
    }

    public async Task<List<GetMenuLabelsResponse>> GetMenuLabelsAsync(CancellationToken ct)
    {
        return await dbContext.MenuLabels
            .OrderBy(l => l.Name)
            .Select(l => new GetMenuLabelsResponse(l.LabelId, l.Name))
            .ToListAsync(ct);
    }
}