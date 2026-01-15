using System.Net;
using NUnit.Framework;
using YTH_backend.DTOs.User;
using YTH_backend.Tests.Common.Assertions;
using YTH_backend.Tests.Common.Factories;
using YTH_backend.Tests.Common.Factories.Debug;
using YTH_backend.Tests.Infrastructure;

namespace YTH_backend.Tests.Tests.Auth;

[TestFixture]
public class LoginTests
{
    private static async Task<HttpResponseMessage> Login(string login, string password) =>
        await ClientCreatingFixture.ApiClient.Auth.Login(new LoginUserRequestDto(login, password));

    private async Task<(string email, string password)> CreateUser()
    {
        var email = ElementaryFactory.CreateEmail();
        var password = ElementaryFactory.CreatePassword();
        await ClientCreatingFixture.ApiClient.Debug.AddUser(
            DebugUserDtoFactory.Create(email: email, password: password));
        return (email, password);
    }

    [Test]
    public async Task ValidCredentials_Returns200AndValidAccessToken()
    {
        var (email, password) = await CreateUser();
        var response = await Login(email, password);
        var token = await AccessTokenReader.Read(response);

        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
        JwtAssertions.CheckToken(token, email, "student");
    }

    [Test]
    public async Task ValidCredentials_SetsRefreshTokenCookie()
    {
        var (email, password) = await CreateUser();
        var response = await Login(email, password);
        CookieAssertions.RefreshTokenIsValid(response);
    }


    [Test]
    public async Task WrongPassword_Returns422()
    {
        var (email, _) = await CreateUser();
        var response = await Login(email, "WrongPassword!1");
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.UnprocessableEntity));
    }

    [Test]
    public async Task NonExistingEmail_Returns422()
    {
        var response = await Login(ElementaryFactory.CreateEmail(), ElementaryFactory.CreatePassword());
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.UnprocessableEntity));
    }
}