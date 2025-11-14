using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;

namespace YTH_backend.Infrastructure;

public static class JwtHelper
{
    public static string GenerateVerificationToken(string email, string secretKey, int expireMinutes = 60)
    {
        var now = DateTime.UtcNow;
        var jti = Guid.NewGuid().ToString();

        var claims = new List<Claim>
        {
            new Claim("ver", "1"),
            new Claim(JwtRegisteredClaimNames.Jti, jti),
            new Claim(JwtRegisteredClaimNames.Iat, ((DateTimeOffset)now).ToUnixTimeSeconds().ToString()),
            new Claim("roles", "with_confirmed_email"),
            new Claim("context", System.Text.Json.JsonSerializer.Serialize(new { email }))
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
}