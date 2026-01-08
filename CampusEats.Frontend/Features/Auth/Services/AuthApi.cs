using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using CampusEats.Frontend.Models;
using CampusEats.Frontend.Shared.Http;

namespace CampusEats.Frontend.Services.Auth;

public class AuthApi(HttpClient http, LoggingHttpClient logger, ITokenService tokenService) : IAuthApi
{
    public async Task<RegisterResponse?> Register(RegisterRequest request)
    {
        logger.LogRequest("POST", "/auth/register", request);

        var response = await http.PostAsJsonAsync("/auth/register", request);
        logger.LogResponse(response);

        if (!response.IsSuccessStatusCode)
        {
            var errorContent = await response.Content.ReadAsStringAsync();
            string message = errorContent;

            try
            {
                using var doc = JsonDocument.Parse(errorContent);
                var root = doc.RootElement;
                if (root.ValueKind == JsonValueKind.Object)
                {
                    if (root.TryGetProperty("errors", out var errorsProp))
                    {
                        if (errorsProp.ValueKind == JsonValueKind.String)
                            message = errorsProp.GetString() ?? message;
                        else
                            message = errorsProp.ToString();
                    }
                    else if (root.TryGetProperty("error", out var errProp))
                    {
                        if (errProp.ValueKind == JsonValueKind.String)
                            message = errProp.GetString() ?? message;
                        else
                            message = errProp.ToString();
                    }
                }
            }
            catch { /* ignore parse errors, use raw content */ }

            throw new Exception(string.IsNullOrWhiteSpace(message) ? "Registration failed." : message);
        }

        return await response.Content.ReadFromJsonAsync<RegisterResponse>();
    }


    public async Task<LoginResponse?> Login(LoginRequest request)
    {
        logger.LogRequest("POST", "/auth/login", request);
        var response = await http.PostAsJsonAsync("/auth/login", request);
        logger.LogResponse(response);

        if (!response.IsSuccessStatusCode)
        {
            var error = await response.Content.ReadAsStringAsync();
            throw new Exception(string.IsNullOrWhiteSpace(error) ? "Login failed." : error);
        }

        var login = await response.Content.ReadFromJsonAsync<LoginResponse>();

        if (login != null && !string.IsNullOrWhiteSpace(login.AccessToken))
        {
            await tokenService.SetTokenAsync(login.AccessToken);
        }

        return login;
    }

    public async Task SaveToken(string token)
    {
        await tokenService.SetTokenAsync(token);
    }

}

