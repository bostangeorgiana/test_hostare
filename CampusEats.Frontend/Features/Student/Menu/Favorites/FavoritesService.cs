using System.Net.Http.Json;
using CampusEats.Frontend.Features.Menu.Models;
using CampusEats.Frontend.Models;

namespace CampusEats.Frontend.Features.Student.Menu.Favorites;

public class FavoritesService
{
    private readonly HttpClient _http;

    public FavoritesService(HttpClient http)
    {
        _http = http;
    }

    public async Task<List<MenuItemDto>> GetFavoritesAsync(int studentId)
    {
        var result = await _http.GetFromJsonAsync<List<MenuItemDto>>(
            $"/api/menu/favorites/{studentId}");

        return result ?? new List<MenuItemDto>();
    }
}