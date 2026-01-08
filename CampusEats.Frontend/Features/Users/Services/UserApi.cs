using System.Net.Http.Json;
using CampusEats.Frontend.Models;

namespace CampusEats.Frontend.Features.Users.Services;

public class UserApi
{
    private readonly HttpClient _http;

    public UserApi(HttpClient http)
    {
        _http = http;
    }

    public Task<UserProfileDto?> GetMyProfile()
        => _http.GetFromJsonAsync<UserProfileDto>("/users/me/profile");

    public Task<UserProfileDto?> GetProfileById(int id)
        => _http.GetFromJsonAsync<UserProfileDto>($"/users/{id}/profile");
}
