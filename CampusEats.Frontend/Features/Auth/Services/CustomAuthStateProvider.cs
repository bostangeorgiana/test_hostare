using System.Security.Claims;
using System.Text.Json;
using System.Text;
using Microsoft.AspNetCore.Components.Authorization;

namespace CampusEats.Frontend.Services.Auth;

public class CustomAuthStateProvider : AuthenticationStateProvider
{
    private readonly ITokenService _tokenService;

    public CustomAuthStateProvider(ITokenService tokenService)
    {
        _tokenService = tokenService;
    }

    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        var token = await _tokenService.GetTokenAsync();
        ClaimsPrincipal principal;

        if (string.IsNullOrWhiteSpace(token))
        {
            principal = new ClaimsPrincipal(new ClaimsIdentity());
        }
        else
        {
            var identity = new ClaimsIdentity(ParseClaimsFromJwt(token), "jwt");
            principal = new ClaimsPrincipal(identity);
        }

        return new AuthenticationState(principal);
    }
    
    public void MarkUserAsAuthenticated(string token)
    {
        var identity = new ClaimsIdentity(ParseClaimsFromJwt(token), "jwt");
        var user = new ClaimsPrincipal(identity);

        NotifyAuthenticationStateChanged(
            Task.FromResult(new AuthenticationState(user))
        );
    }

    public void MarkUserAsLoggedOut()
    {
        var anonymous = new ClaimsPrincipal(new ClaimsIdentity());
        NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(anonymous)));
    }

    private IEnumerable<Claim> ParseClaimsFromJwt(string jwt)
    {
        var claims = new List<Claim>();

        var parts = jwt.Split('.');
        if (parts.Length < 2)
            return claims;

        string payload = parts[1];

        payload = payload.Replace('-', '+').Replace('_', '/');
        switch (payload.Length % 4)
        {
            case 2: payload += "=="; break;
            case 3: payload += "="; break;
        }

        try
        {
            var bytes = Convert.FromBase64String(payload);
            var json = Encoding.UTF8.GetString(bytes);
            using var doc = JsonDocument.Parse(json);

            foreach (var prop in doc.RootElement.EnumerateObject())
            {
                if (prop.Name == "sub") 
                    claims.Add(new Claim(ClaimTypes.NameIdentifier, prop.Value.GetString() ?? ""));
                else if (prop.Name == "name") 
                    claims.Add(new Claim(ClaimTypes.Name, prop.Value.GetString() ?? ""));
                else if (prop.Name == "email") 
                    claims.Add(new Claim(ClaimTypes.Email, prop.Value.GetString() ?? ""));
                else if (prop.Name == "role") 
                    claims.Add(new Claim(ClaimTypes.Role, prop.Value.GetString() ?? ""));
                else
                    claims.Add(new Claim(prop.Name, prop.Value.ToString()));
            }
        }
        catch 
        { }

        return claims;
    }
}