using System.Net;
using NUnit.Framework;
using YTH_backend.Features.Debug;
using YTH_backend.Tests.Common.Assertions;

namespace YTH_backend.Tests.Tests.Debug;

[TestFixture]
public class GetTokenWithConfirmedEmailTests
{
    private Task<HttpResponseMessage> Post(string email) =>
        ClientCreatingFixture.ApiClient.Debug.GetTokenWithConfirmedEmail(new GetTokenWithConfirmedEmailDto(email));

    [Test]
    public async Task ValidEmail_Returns200AndToken()
    {
        var email = $"email_{Guid.NewGuid():N}@test.com";
        var response = await Post(email);
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
        var token = await response.Content.ReadAsStringAsync();
        Assert.That(token, Is.Not.Empty);
    }

    [Test]
    public async Task Jwt_IsValid()
    {
        var email = $"email_{Guid.NewGuid():N}@test.com";
        var response = await Post(email);
        var token = await response.Content.ReadAsStringAsync();
        JwtAssertions.IsValidBaseClaims(token);
    }

    [Test]
    public async Task Jwt_HasWithConfirmedEmailRole()
    {
        var email = $"email_{Guid.NewGuid():N}@test.com";
        var response = await Post(email);
        var token = await response.Content.ReadAsStringAsync();
        JwtAssertions.HasRole(token, "with_confirmed_email");
    }

    [Test]
    public async Task Jwt_ContainsEmailInContext()
    {
        var email = $"email_{Guid.NewGuid():N}@test.com";
        var response = await Post(email);
        var token = await response.Content.ReadAsStringAsync();
        JwtAssertions.HasEmailInContext(token, email);
    }

    [TestCase("")]
    [TestCase("not-an-email")]
    [TestCase("███好奇心█")]
    [TestCase("email@")]
    public async Task Email_IsNotValidated_StillReturns200(string email)
    {
        var response = await Post(email);
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
    }
}