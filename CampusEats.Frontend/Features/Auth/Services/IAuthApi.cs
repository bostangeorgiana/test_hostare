using CampusEats.Frontend.Models;

namespace CampusEats.Frontend.Services.Auth;

public interface IAuthApi
{
    Task<RegisterResponse?> Register(RegisterRequest request);
    Task<LoginResponse?> Login(LoginRequest request);
    Task SaveToken(string token);
}