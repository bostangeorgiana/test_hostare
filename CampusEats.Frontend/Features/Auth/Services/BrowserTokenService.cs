using Microsoft.JSInterop;

namespace CampusEats.Frontend.Services.Auth;

public class BrowserTokenService : ITokenService
{
    private const string TokenKey = "campuseats_access_token";
    private readonly IJSRuntime _js;

    public BrowserTokenService(IJSRuntime js)
    {
        _js = js;
    }

    public async Task SetTokenAsync(string token)
    {
        await _js.InvokeVoidAsync("localStorage.setItem", TokenKey, token);
    }

    public async Task<string?> GetTokenAsync()
    {
        return await _js.InvokeAsync<string?>("localStorage.getItem", TokenKey);
    }

    public async Task RemoveTokenAsync()
    {
        await _js.InvokeVoidAsync("localStorage.removeItem", TokenKey);
    }
}
