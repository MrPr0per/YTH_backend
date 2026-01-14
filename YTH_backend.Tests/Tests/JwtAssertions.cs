using System.IdentityModel.Tokens.Jwt;
using NUnit.Framework;

namespace YTH_backend.Tests.Tests;

public static class JwtAssertions
{
    private static readonly JwtSecurityTokenHandler Handler = new();

    public static void HasRole(string jwt, string role)
    {
        var rolesJson = Handler.ReadJwtToken(jwt).Claims.First(c => c.Type == "roles").Value;
        var roles = System.Text.Json.JsonSerializer.Deserialize<List<string>>(rolesJson);
        Assert.That(roles, Does.Contain(role));
    }

    public static void IsValidBaseClaims(string jwt)
    {
        var token = Handler.ReadJwtToken(jwt);

        Assert.That(token.Payload.ContainsKey("iat"));
        Assert.That(token.Payload.ContainsKey("exp"));
        Assert.That(token.Payload.ContainsKey("jti"));
        Assert.That(token.Payload.ContainsKey("ver"));
    }
}