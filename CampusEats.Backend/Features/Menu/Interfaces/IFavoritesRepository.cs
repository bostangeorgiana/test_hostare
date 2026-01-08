namespace CampusEats.Features.Menu.Interfaces;

public interface IFavoritesRepository
{
    Task<bool> ExistsAsync(int studentId, int menuItemId);
    Task AddAsync(int studentId, int menuItemId);
    Task RemoveAsync(int studentId, int menuItemId);
    Task<List<int>> GetStudentsWhoFavoritedAsync(int menuItemId);
    Task<List<int>> GetFavoriteMenuItemIdsAsync(int studentId);
}