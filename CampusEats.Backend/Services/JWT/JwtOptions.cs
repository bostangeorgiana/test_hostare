namespace CampusEats.Services.JWT;

/**
 * JwtOptions defines the configuration settings used to create and validate JWT tokens.
 *
 * Properties:
 * - Key: The secret key used to sign and verify the JWT.
 * - Issuer: Identifies who issued the JWT (e.g., your API or authentication server).
 * - Audience: Identifies who the JWT is intended for (e.g., your frontend or API clients).
 * - ExpiresMinutes: The number of minutes until the JWT expires after being issued.
 *
 * These options are used when generating tokens and validating incoming requests.
 * 
 * Common JWT reserved claims that use these values:
 * - iss (issuer): Matches the Issuer property.
 * - aud (audience): Matches the Audience property.
 * - exp (expiration): Derived from ExpiresMinutes.
 * - iat (issued at): Automatically set when the token is created.
 */

public class JwtOptions
{
    public string Key { get; set; } = null!;
    public string Issuer { get; set; } = null!;
    public string Audience { get; set; } = null!;
    public int ExpiresMinutes { get; set; }
}