using CampusEats.Frontend.Features.Menu.Models;
using CampusEats.Frontend.Models;
using CampusEats.Frontend.Features.Menu.Services;

namespace CampusEats.Frontend.Features.Student.Cart.Services;

public class CartService
{
    private readonly MenuApi _menuApi;

    public CampusEats.Frontend.Models.Cart Cart { get; private set; } = new CampusEats.Frontend.Models.Cart();

    public event Action? OnChange;
    public CartService(MenuApi menuApi)
    {
        _menuApi = menuApi;
    }
    
    private void AddItemInternal(int id, string name, decimal price, int quantity = 1)
    {
        var existing = Cart.Items.FirstOrDefault(x => x.MenuItemId == id);

        if (existing != null)
            existing.Quantity += quantity;
        else
            Cart.Items.Add(new CartItem
            {
                MenuItemId = id,
                Name = name,
                Price = price,
                Quantity = quantity
            });

        OnChange?.Invoke();
    }
    public async Task<(bool Success, string? Error)> TryAddItemAsync(int id, string name, decimal price, int quantity = 1)
    {
        var menuItem = await _menuApi.GetMenuItemAsync(id);
        if (menuItem == null)
            return (false, "You have reached the maximum available quantity for this product.");

        int currentStock = menuItem.CurrentStock;

        var existing = Cart.Items.FirstOrDefault(x => x.MenuItemId == id);
        int inCart = existing?.Quantity ?? 0;

        if (inCart + quantity > currentStock)
            return (false, "You have reached the maximum available quantity for this product.");
        
        AddItemInternal(id, name, price, quantity);
        return (true, null);
    }
    
    public async Task<(bool Success, string? Error)> TryAddItemAsync(MenuItemDto item, int quantity = 1)
    {
        return await TryAddItemAsync(item.MenuItemId, item.Name, item.Price, quantity);
    }

    public void RemoveItem(int id)
    {
        var item = Cart.Items.FirstOrDefault(x => x.MenuItemId == id);
        if (item != null)
        {
            Cart.Items.Remove(item);
            OnChange?.Invoke();
        }
    }

    public void Clear()
    {
        Cart.Items.Clear();
        OnChange?.Invoke();
    }

    public async Task PlaceOrderAsync(string? notes)
    {
        var order = new
        {
            Items = Cart.Items.Select(item => new
            {
                item.MenuItemId,
                item.Quantity
            }),
            Notes = notes
        };
        
        await Task.Delay(500); 
        
        Clear();
    }
    // Update quantity directly
    public void UpdateQuantity(int id, int quantity)
    {
        var item = Cart.Items.FirstOrDefault(x => x.MenuItemId == id);
        if (item != null)
        {
            if (quantity <= 0)
                Cart.Items.Remove(item);
            else
                item.Quantity = quantity;

            OnChange?.Invoke();
        }
    }

    // Increment quantity
    public void IncrementQuantity(int id)
    {
        var item = Cart.Items.FirstOrDefault(x => x.MenuItemId == id);
        if (item != null)
        {
            item.Quantity++;
            OnChange?.Invoke();
        }
    }

    // Decrement quantity
    public void DecrementQuantity(int id)
    {
        var item = Cart.Items.FirstOrDefault(x => x.MenuItemId == id);
        if (item != null)
        {
            item.Quantity--;
            if (item.Quantity <= 0)
                Cart.Items.Remove(item);

            OnChange?.Invoke();
        }
    }
}