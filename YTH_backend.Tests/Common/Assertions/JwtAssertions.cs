using System.IdentityModel.Tokens.Jwt;
using NUnit.Framework;

namespace YTH_backend.Tests.Common.Assertions;

public static class JwtAssertions
{
    private static readonly JwtSecurityTokenHandler Handler = new();

    public static void CheckToken(string jwt, string email, string role)
    {
        IsValidBaseClaims(jwt);
        HasEmailInContext(jwt, email);
        HasRole(jwt, role);
    }

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

    public static void HasEmailInContext(string jwt, string email)
    {
        var contextStr = Handler.ReadJwtToken(jwt).Claims.First(c => c.Type == "context").Value;
        var contextDict = System.Text.Json.JsonSerializer.Deserialize<Dictionary<string, string>>(contextStr);
        Assert.That(contextDict, Is.Not.Null);
        Assert.That(contextDict, Contains.Key("email"));
        var emailFromContext = contextDict!["email"];
        Assert.That(emailFromContext, Is.EqualTo(email));
    }
}