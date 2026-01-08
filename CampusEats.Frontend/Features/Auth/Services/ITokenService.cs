// ...new file...
namespace CampusEats.Frontend.Services.Auth;

public interface ITokenService
{
    Task SetTokenAsync(string token);
    Task<string?> GetTokenAsync();
    Task RemoveTokenAsync();
}

