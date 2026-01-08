using System.Security.Claims;
namespace CampusEats.Services.JWT;

/**
 * Jwt Token Service Interface
 * Here we define methods that must be implemented in the JwtService
 */
public interface IJwtTokenService
{
    /**
     * Token creation method
     * @param userId: the user's id
     * @param email: the user's email
     * @param role: the user's role
     */
    string CreateAccessToken(int userId, string email, string role);
    
    string CreateRefreshToken();
    
    ClaimsPrincipal? GetPrincipalFromExpiredToken(string token);
}