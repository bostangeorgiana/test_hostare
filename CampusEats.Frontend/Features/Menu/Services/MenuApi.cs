using System.Net.Http.Json;
using CampusEats.Frontend.Features.Menu.Models;

namespace CampusEats.Frontend.Features.Menu.Services;

public class MenuApi(HttpClient http)
{
    private readonly HttpClient _http = http;
    
    public async Task<List<MenuItemDto>?> GetMenuAsync(
        bool onlyAvailable = false,
        List<int>? labels = null,
        bool onlyFavorites = false,
        decimal? minPrice = null,
        decimal? maxPrice = null)
    {
        var query = new List<string>();

        if (onlyAvailable)
            query.Add("onlyAvailable=true");
        
        query.Add($"onlyFavorites={onlyFavorites.ToString().ToLower()}");

        if (minPrice is not null)
            query.Add($"minPrice={minPrice.Value}");

        if (maxPrice is not null)
            query.Add($"maxPrice={maxPrice.Value}");

        if (labels is { Count: > 0 })
        {
            foreach (var l in labels)
                query.Add($"labels={l}");
        }

        string url = "/api/menu";
        if (query.Count > 0)
            url += "?" + string.Join("&", query);

        return await _http.GetFromJsonAsync<List<MenuItemDto>>(url);
    }

    public async Task<MenuItemDto?> GetMenuItemAsync(int id)
        => await _http.GetFromJsonAsync<MenuItemDto>($"/api/menu/{id}");

    public async Task<int?> CreateMenuItemAsync(CreateMenuItemDto dto)
    {
        var response = await _http.PostAsJsonAsync("/api/menu", dto);
        if (!response.IsSuccessStatusCode)
            return null;

        return await response.Content.ReadFromJsonAsync<int>();
    }

    public async Task<bool> UpdateMenuItemAsync(MenuItemDto item)
    {
        var response = await _http.PutAsJsonAsync($"/api/menu/{item.MenuItemId}", item);
        return response.IsSuccessStatusCode;
    }

    public async Task<bool> DeleteMenuItemAsync(int id)
    {
        var response = await _http.DeleteAsync($"/api/menu/{id}");
        return response.IsSuccessStatusCode;
    }

    public async Task UpdateStockAsync(int menuItemId, int newStock)
    {
        await _http.PatchAsync($"/api/menu/{menuItemId}/stock?newStock={newStock}", null);
    }

    public async Task ToggleFavoriteAsync(int menuItemId, int studentId)
    {
        var cmd = new { MenuItemId = menuItemId, StudentId = studentId };
        await _http.PostAsJsonAsync("/api/menu/favorite", cmd);
    }

    public async Task<List<MenuLabelDto>?> GetLabelsAsync()
    {
        return await _http.GetFromJsonAsync<List<MenuLabelDto>>("/api/menu/labels");
    }
    
    public async Task<List<IngredientListDto>?> GetIngredientsAsync()
        => await _http.GetFromJsonAsync<List<IngredientListDto>>("/api/menu/ingredients");
}