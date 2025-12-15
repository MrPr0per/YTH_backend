using System.Net;
using System.Net.Http.Json;
using NUnit.Framework;
using YTH_backend.DTOs.User;

namespace YTH_backend.Tests.Tests;

[TestFixture]
public class AuthTests
{
    private HttpClient client;

    [OneTimeSetUp]
    public async Task OneTimeSetUp()
    {
        client = PostgresTestcontainerFixture.Factory.CreateClient();

        // создаём пользователя через debug endpoint
        var response = await client.PostAsJsonAsync(
            "/api/v0/debug/addUser",
            new
            {
                username = "testuser",
                password = "Password123!",
                email = "test@test.com",
                role = "Student"
            });

        response.EnsureSuccessStatusCode();
    }

    [OneTimeTearDown]
    public void OneTimeTearDown()
    {
        client.Dispose();
        // TODO: добавить удаление тестового пользователя
    }

    [Test]
    public async Task Login_WithCorrectCredentials_ReturnsAccessToken_AndRefreshCookie()
    {
        // Act
        var response = await client.PostAsJsonAsync(
            "/api/v0/auth/login",
            new LoginUserRequestDto(
                Login: "test@test.com",
                Password: "Password123!"
            ));

        // Assert
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));

        var body = await response.Content.ReadFromJsonAsync<Dictionary<string, string>>();
        Assert.That(body, Is.Not.Null);
        Assert.That(body!["access_token"], Is.Not.Empty);

        // refresh token должен быть в cookie
        Assert.That(
            response.Headers.TryGetValues("Set-Cookie", out var cookies),
            Is.True);

        Assert.That(
            cookies!.Any(c => c.StartsWith("refreshToken")),
            Is.True);

        // todo: проверять jwt токен на уровень доступа
    }

    [Test]
    public async Task Login_WithWrongPassword_Returns422()
    {
        var response = await client.PostAsJsonAsync(
            "/api/v0/auth/login",
            new LoginUserRequestDto(
                Login: "test@test.com",
                Password: "WrongPassword"
            ));

        Assert.That(response.StatusCode, Is.EqualTo((HttpStatusCode)422));
    }

    [Test]
    public async Task Login_WithNonExistingEmail_Returns422()
    {
        var response = await client.PostAsJsonAsync(
            "/api/v0/auth/login",
            new LoginUserRequestDto(
                Login: "notfound@test.com",
                Password: "Password123!"
            ));

        Assert.That(response.StatusCode, Is.EqualTo((HttpStatusCode)422));
    }

    [Test]
    public async Task Login_WithInvalidBody_Returns400()
    {
        // отсутствует пароль
        var response = await client.PostAsJsonAsync(
            "/api/v0/auth/login",
            new
            {
                email = "test@test.com"
            });

        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
    }
}