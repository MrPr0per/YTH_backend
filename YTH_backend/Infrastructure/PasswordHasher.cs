using System.Security.Cryptography;
using System.Text;

namespace YTH_backend.Infrastructure;

public static class PasswordHasher
{
    public static string GenerateSalt()
    {
        var salt = RandomNumberGenerator.GetBytes(16);
        return Convert.ToBase64String(salt);
    }

    public static string HashPassword(string password, string salt)
    {
        using var sha256 = SHA256.Create();
        var combined = Encoding.UTF8.GetBytes(password + salt);
        var hash = sha256.ComputeHash(combined);
        
        return Convert.ToBase64String(hash);
    }
}