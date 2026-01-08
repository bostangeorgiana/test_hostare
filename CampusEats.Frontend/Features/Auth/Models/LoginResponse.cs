namespace CampusEats.Frontend.Models;

public class LoginResponse
{
    public string AccessToken { get; set; } = "";
    public string TokenType { get; set; } = "";
    public int ExpiresIn { get; set; }
    public LoginUserInfo User { get; set; } = new();
}