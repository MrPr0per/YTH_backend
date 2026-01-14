using System.Net;
using System.Net.Http.Json;
using NUnit.Framework;

namespace YTH_backend.Tests.Tests;

public static class DebugUserFactory
{
    /// <param name="username">по умолчанию - user_{guid:N}</param>
    /// <param name="email">по умолчанию - email_{guid:N}@test.com</param>
    /// <param name="role"></param>
    public static object GetUserObject(
        string? username = null,
        string? email = null,
        string role = "student"
    )
    {
        return new
        {
            username = username ?? $"user_{Guid.NewGuid():N}",
            password = "Password123!",
            email = email ?? $"email_{Guid.NewGuid():N}@test.com",
            role
        };
    }
}

[TestFixture]
public class DebugAddUserTests
{
    private const string Url = "/api/v0/debug/addUser";

    [Test]
    public async Task AddUser_Success_ReturnsAccessToken()
    {
        var response = await PostgresTestcontainerFixture.Client.PostAsJsonAsync(
            Url,
            DebugUserFactory.GetUserObject()
        );

        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));

        var token = await response.Content.ReadAsStringAsync();
        Assert.That(token, Is.Not.Empty);
    }

    [Test]
    public async Task AddUser_DuplicateUsername_Returns409()
    {
        var username = $"dup_{Guid.NewGuid():N}";
        var first = DebugUserFactory.GetUserObject(username: username);
        var second = DebugUserFactory.GetUserObject(username: username);

        await PostgresTestcontainerFixture.Client.PostAsJsonAsync(Url, first);
        var response = await PostgresTestcontainerFixture.Client.PostAsJsonAsync(Url, second);

        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.Conflict));
    }

    [Test]
    public async Task AddUser_DuplicateEmail_Returns409()
    {
        var email = $"dup_{Guid.NewGuid():N}@test.com";
        var first = DebugUserFactory.GetUserObject(email: email);
        var second = DebugUserFactory.GetUserObject(email: email);

        await PostgresTestcontainerFixture.Client.PostAsJsonAsync(Url, first);
        var response = await PostgresTestcontainerFixture.Client.PostAsJsonAsync(Url, second);

        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.Conflict));
    }

    [Test]
    public async Task AddUser_ReturnsValidJwt()
    {
        var response = await PostgresTestcontainerFixture.Client.PostAsJsonAsync(
            Url,
            DebugUserFactory.GetUserObject());
        var token = await response.Content.ReadAsStringAsync();
        JwtAssertions.IsValidBaseClaims(token);
    }

    [TestCase("student")]
    [TestCase("admin")]
    [TestCase("superadmin")]
    [TestCase("expert")]
    public async Task AddUser_RoleIsPresentInJwt(string role)
    {
        var response = await PostgresTestcontainerFixture.Client.PostAsJsonAsync(
            Url,
            DebugUserFactory.GetUserObject(role: role));
        var token = await response.Content.ReadAsStringAsync();
        JwtAssertions.HasRole(token, role);
    }

    [Test]
    public async Task AddUser_InvalidRole_DefaultsToStudent_Returns200()
    {
        var response = await PostgresTestcontainerFixture.Client.PostAsJsonAsync(
            Url,
            DebugUserFactory.GetUserObject(role: "invalid"));
        var token = await response.Content.ReadAsStringAsync();
        JwtAssertions.HasRole(token, "student");
    }
}