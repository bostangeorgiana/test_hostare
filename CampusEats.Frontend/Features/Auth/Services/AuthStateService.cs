namespace CampusEats.Frontend.Services.Auth;

public class AuthStateService
{
    public int UserId { get; private set; }
    public string Role { get; private set; } = "";
    public string Email { get; private set; } = "";

    public event Action? OnChange;

    private void NotifyStateChanged() => OnChange?.Invoke();

    public void SetUser(int userId, string role, string email)
    {
        UserId = userId;
        Role = role;
        Email = email;

        NotifyStateChanged();
    }

    public void Clear()
    {
        UserId = 0;
        Role = "";
        Email = "";

        NotifyStateChanged();
    }
}