using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using Microsoft.IdentityModel.Tokens;

namespace YTH_backend.Infrastructure;

public static class JwtHelper
{
    public static string GenerateVerificationToken(IDictionary<string, object> contextData, string secretKey, int expireMinutes = 10, IEnumerable<string>? roles = null)
    {
        var now = DateTime.UtcNow;
        var jti = Guid.NewGuid().ToString();
        var rolesList = roles?.ToList() ?? new List<string> { "with_confirmed_email" };

        var claims = new List<Claim>
        {
            new Claim("ver", "1"),
            new Claim(JwtRegisteredClaimNames.Jti, jti),
            new Claim(JwtRegisteredClaimNames.Iat, ((DateTimeOffset)now).ToUnixTimeSeconds().ToString()),
            new Claim("roles", System.Text.Json.JsonSerializer.Serialize(rolesList)),
            new Claim("context", System.Text.Json.JsonSerializer.Serialize(contextData))
        };

        var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(secretKey));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            claims: claims,
            notBefore: now,
            expires: now.AddMinutes(expireMinutes),
            signingCredentials: creds
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
    
    public static string GenerateRefreshToken() 
        => Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));
}