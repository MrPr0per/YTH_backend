using System.Net;
using NUnit.Framework;
using YTH_backend.Features.Debug;
using YTH_backend.Tests.Common.Assertions;
using YTH_backend.Tests.Common.Factories;
using YTH_backend.Tests.Common.ValidationCases;
using YTH_backend.Tests.Infrastructure;

namespace YTH_backend.Tests.Tests.Debug;

[TestFixture]
public class GetTokenWithConfirmedEmailTests
{
    private static Task<HttpResponseMessage> Post(string? email = null) =>
        ClientCreatingFixture.ApiClient.Debug.GetTokenWithConfirmedEmail(
            new GetTokenWithConfirmedEmailDto(
                email ?? ElementaryFactory.CreateEmail()
            )
        );

    [Test]
    public async Task ValidEmail_Returns200AndToken()
    {
        var response = await Post();
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
        var token = await response.Content.ReadAsStringAsync();
        Assert.That(token, Is.Not.Empty);
    }

    [Test]
    public async Task Jwt_IsValid()
    {
        var response = await Post();
        var token = await response.Content.ReadAsStringAsync();
        JwtAssertions.IsValidBaseClaims(token);
    }

    [Test]
    public async Task Jwt_HasWithConfirmedEmailRole()
    {
        var response = await Post();
        var token = await response.Content.ReadAsStringAsync();
        JwtAssertions.HasRole(token, "with_confirmed_email");
    }

    [Test]
    public async Task Jwt_ContainsEmailInContext()
    {
        var email = ElementaryFactory.CreateEmail();
        var response = await Post(email);
        var token = await response.Content.ReadAsStringAsync();
        JwtAssertions.HasEmailInContext(token, email);
    }

    [TestCaseSource(typeof(EmailValidationCases), nameof(EmailValidationCases.All))]
    public async Task EmailValidationTests(ValidationCase<string> validationCase) =>
        await ValidationAssertion.Run(validationCase, Post);
}