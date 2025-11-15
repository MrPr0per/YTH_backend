using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using YTH_backend.Enums;
using YTH_backend.Models.User;

namespace YTH_backend.Infrastructure;

public static class JwtHelper
{
    /// <summary>
    /// Генератор JWT токена
    /// </summary>
    /// <param name="contextData">То что будет лежать в context</param>
    /// <param name="secretKey">Секретный ключ</param>
    /// <param name="expireMinutes">Время действия</param>
    /// <param name="roles">Список ролей пользователя</param>
    /// <returns>JWT</returns>
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

    /// <summary>
    /// Перегрузка оригинального метода с дефолтными значениями
    /// </summary>
    /// <param name="user">Пользователь</param>
    /// <param name="secretKey">Секретный ключ</param>
    /// <returns>JWT</returns>
    public static string GenerateVerificationToken(User user, string secretKey)
    {
        var contextData = new Dictionary<string, object>
        {
            ["email"] = user.Email,
            ["id"] = user.Id
        };
        
        var rolesList = SelectRoles(user);
        
        return GenerateVerificationToken(
            contextData,
            secretKey,
            10,
            rolesList
        );
    }
    
    
    public static IDictionary<string, object> DecodeToken(string token, string secretKey)
    {
        var handler = new JwtSecurityTokenHandler();
        var key = Encoding.UTF8.GetBytes(secretKey);

        var validationParams = new TokenValidationParameters
        {
            ValidateIssuer = false,
            ValidateAudience = false,
            ValidateLifetime = false, // нам важно просто получить payload
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(key)
        };

        handler.ValidateToken(token, validationParams, out var validatedToken);

        var jwtToken = validatedToken as JwtSecurityToken;
        if (jwtToken == null)
            throw new Exception("Invalid token");

        return jwtToken.Claims.ToDictionary(c => c.Type, c => (object)c.Value);
    }
    
    /// <summary>
    /// Генератор Refresh токена
    /// </summary>
    /// <returns>Refresh токен</returns>
    public static string GenerateRefreshToken() 
        => Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));

    /// <summary>
    /// Хэширует токен
    /// </summary>
    /// <param name="token">Токен</param>
    /// <returns>Хэш токена</returns>
    public static string HashRefreshToken(string token)
    {
        using var sha256 = SHA256.Create();
        var hashedRefreshToken = sha256.ComputeHash(Encoding.UTF8.GetBytes(token));
        
        return Convert.ToBase64String(hashedRefreshToken);
    }

    /// <summary>
    /// Метод для выбора ролей для JWT на основе роли в бд
    /// </summary>
    /// <param name="user">Пользователь</param>
    /// <returns>Список ролей</returns>
    public static List<string> SelectRoles(User user)
    {
        var rolesList = new List<string> {"logged_in"};

        switch (user.Role)
        {
            case Roles.Admin:
                rolesList.Add("admin");
                break;
            case Roles.SuperAdmin:
                rolesList.Add("superadmin");
                break;
            case Roles.Student:
                rolesList.Add("student");
                break;
        }
        
        return rolesList;
    }
    
    
}