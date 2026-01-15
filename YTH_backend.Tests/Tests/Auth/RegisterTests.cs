using System.Net;
using System.Net.Http.Json;
using NUnit.Framework;
using YTH_backend.Features.Debug;
using YTH_backend.Tests.Common.Assertions;
using YTH_backend.Tests.Common.Factories;
using YTH_backend.Tests.Common.Factories.Auth;
using YTH_backend.Tests.Common.Factories.Debug;
using YTH_backend.Tests.Common.ValidationCases;
using YTH_backend.Tests.Infrastructure;

namespace YTH_backend.Tests.Tests.Auth;

[TestFixture]
public class RegisterTests
{
    private static async Task<HttpResponseMessage> Register(
        string? email = null,
        string? username = null,
        string? password = null)
    {
        email ??= ElementaryFactory.CreateEmail();

        var emailTokenResponse = await ClientCreatingFixture.ApiClient.Debug.GetTokenWithConfirmedEmail(
            new GetTokenWithConfirmedEmailDto(email)
        );

        var emailToken = await emailTokenResponse.Content.ReadAsStringAsync();

        return await ClientCreatingFixture.ApiClient.Auth.Register(
            RegisterFactory.Create(username, password),
            emailToken
        );
    }

    private static async Task<string> ReadToken(HttpResponseMessage response)
    {
        var responseJson = await response.Content.ReadFromJsonAsync<Dictionary<string, string>>();
        Assert.That(responseJson, Is.Not.Null);
        Assert.That(responseJson, Has.Count.EqualTo(1));
        Assert.That(responseJson, Contains.Key("access_token"));
        return responseJson!["access_token"];
    }

    [Test]
    public async Task ValidData_Returns201AndAccessToken()
    {
        var response = await Register();
        var token = await ReadToken(response);
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.Created));
        Assert.That(token, Is.Not.Empty);
    }

    [Test]
    public async Task ValidData_SetsRefreshTokenCookie()
    {
        var response = await Register();
        CookieAssertions.RefreshTokenIsValid(response);
    }

    [Test]
    public async Task Jwt_HasLoggedInAndStudentRoles()
    {
        var token = await ReadToken(await Register());
        JwtAssertions.HasRole(token, "logged_in");
        JwtAssertions.HasRole(token, "student");
    }

    [Test]
    public async Task Jwt_ContainsEmailFromTokenContext()
    {
        var email = ElementaryFactory.CreateEmail();
        var token = await ReadToken(await Register(email));
        JwtAssertions.HasEmailInContext(token, email);
    }

    [Test]
    public async Task DuplicateUsername_Returns409()
    {
        var username = $"dup_{ElementaryFactory.CreateGuid()}";
        await Register(username: username);
        var response = await Register(username: username);
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.Conflict));
    }

    [Test]
    public async Task MissingConfirmedEmailRole_Returns401()
    {
        var response = await ClientCreatingFixture.ApiClient.Auth.Register(
            RegisterFactory.Create(),
            accessToken: "invalid-or-missing-token"
        );
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.Unauthorized));
    }

    [Test]
    public async Task MissingAccessInToken_Returns403()
    {
        var validTokenWithoutAccess =
            await ClientCreatingFixture.ApiClient.Debug.AddUser(DebugUserFactory.Create(role: "student"))
                .Result.Content.ReadAsStringAsync();
        var response = await ClientCreatingFixture.ApiClient.Auth.Register(
            RegisterFactory.Create(),
            accessToken: validTokenWithoutAccess
        );
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.Forbidden));
    }

    [TestCaseSource(typeof(EmailValidationCases), nameof(EmailValidationCases.All))]
    public async Task EmailValidationTests(ValidationCase<string> validationCase) =>
        await ValidationAssertion.Run(validationCase, e => Register(email: e));

    [TestCaseSource(typeof(UsernameValidationCases), nameof(UsernameValidationCases.All))]
    public async Task UsernameValidationTests(ValidationCase<string> validationCase) =>
        await ValidationAssertion.Run(validationCase, u => Register(username: u));

    [TestCaseSource(typeof(PasswordSValidationCases), nameof(PasswordSValidationCases.All))]
    public async Task PasswordValidationTests(ValidationCase<string> validationCase) =>
        await ValidationAssertion.Run(validationCase, p => Register(password: p));
}