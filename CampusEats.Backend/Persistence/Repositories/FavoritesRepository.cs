using CampusEats.Features.Menu.Interfaces;
using CampusEats.Persistence.Context;
using CampusEats.Persistence.Entities;
using Microsoft.EntityFrameworkCore;

namespace CampusEats.Persistence.Repositories;

public class FavoritesRepository(CampusEatsDbContext context) : IFavoritesRepository
{
    public async Task<bool> ExistsAsync(int studentId, int menuItemId)
    {
        return await context.Favorites
            .AnyAsync(f => f.StudentId == studentId && f.MenuItemId == menuItemId);
    }

    public async Task AddAsync(int studentId, int menuItemId)
    {
        var favorite = new Favorite
        {
            StudentId = studentId,
            MenuItemId = menuItemId
        };

        context.Favorites.Add(favorite);
        await context.SaveChangesAsync();
    }

    public async Task RemoveAsync(int studentId, int menuItemId)
    {
        var favorite = await context.Favorites
            .FirstOrDefaultAsync(f => f.StudentId == studentId && f.MenuItemId == menuItemId);

        if (favorite != null)
        {
            context.Favorites.Remove(favorite);
            await context.SaveChangesAsync();
        }
    }

    public async Task<List<int>> GetStudentsWhoFavoritedAsync(int menuItemId)
    {
        return await context.Favorites
            .Where(f => f.MenuItemId == menuItemId)
            .Select(f => f.StudentId)
            .ToListAsync();
    }
    
    public async Task<List<int>> GetFavoriteMenuItemIdsAsync(int studentId)
    {
        return await context.Favorites
            .Where(f => f.StudentId == studentId)
            .Select(f => f.MenuItemId)
            .ToListAsync();
    }
}