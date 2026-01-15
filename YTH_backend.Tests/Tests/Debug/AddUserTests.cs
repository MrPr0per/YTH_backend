using System.Net;
using NUnit.Framework;
using YTH_backend.Tests.Common.Assertions;
using YTH_backend.Tests.Common.Factories.Debug;
using YTH_backend.Tests.Infrastructure;

namespace YTH_backend.Tests.Tests.Debug;

[TestFixture]
public class DebugAddUserTests
{
    [Test]
    public async Task AddUser_Success_ReturnsAccessToken()
    {
        var response = await ClientCreatingFixture.ApiClient.Debug.AddUser(DebugUserFactory.Create());
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
        var token = await response.Content.ReadAsStringAsync();
        Assert.That(token, Is.Not.Empty);
    }

    [Test]
    public async Task AddUser_DuplicateUsername_Returns409()
    {
        var username = $"dup_{Guid.NewGuid():N}";
        var first = DebugUserFactory.Create(username: username);
        var second = DebugUserFactory.Create(username: username);

        await ClientCreatingFixture.ApiClient.Debug.AddUser(first);
        var response = await ClientCreatingFixture.ApiClient.Debug.AddUser(second);

        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.Conflict));
    }

    [Test]
    public async Task AddUser_DuplicateEmail_Returns409()
    {
        var email = $"dup_{Guid.NewGuid():N}@test.com";
        var first = DebugUserFactory.Create(email: email);
        var second = DebugUserFactory.Create(email: email);

        await ClientCreatingFixture.ApiClient.Debug.AddUser(first);
        var response = await ClientCreatingFixture.ApiClient.Debug.AddUser(second);

        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.Conflict));
    }

    [Test]
    public async Task AddUser_ReturnsValidJwt()
    {
        var response = await ClientCreatingFixture.ApiClient.Debug.AddUser(DebugUserFactory.Create());
        var token = await response.Content.ReadAsStringAsync();
        JwtAssertions.IsValidBaseClaims(token);
    }

    [TestCase("student")]
    [TestCase("admin")]
    [TestCase("superadmin")]
    public async Task AddUser_RoleIsPresentInJwt(string role)
    {
        var response = await ClientCreatingFixture.ApiClient.Debug.AddUser(DebugUserFactory.Create(role: role));
        var token = await response.Content.ReadAsStringAsync();
        JwtAssertions.HasRole(token, role);
    }

    [TestCase("invalid")]
    [TestCase("34t8u8__████")]
    [TestCase("expert")]
    public async Task AddUser_InvalidRole_DefaultsToStudent_Returns200(string invalidRole)
    {
        var response = await ClientCreatingFixture.ApiClient.Debug.AddUser(DebugUserFactory.Create(role: invalidRole));
        var token = await response.Content.ReadAsStringAsync();
        JwtAssertions.HasRole(token, "student");
    }
}