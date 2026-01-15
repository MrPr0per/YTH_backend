using System.Net;
using NUnit.Framework;
using YTH_backend.DTOs.User;
using YTH_backend.Tests.Common.Factories.Debug;

namespace YTH_backend.Tests.Tests.Auth;

[TestFixture]
public class SendVerificationEmailForRegistrationTests
{
    private Task<HttpResponseMessage> Post(string email) =>
        ClientCreatingFixture.ApiClient.Auth.SendVerificationEmailForRegistration(
            new SendVerificationEmailRequestDto(email)
        );

    [Test]
    public async Task ValidEmail_Returns204()
    {
        var email = $"email_{Guid.NewGuid():N}@test.com";
        var response = await Post(email);
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.NoContent));
    }

    [TestCase("")]
    [TestCase("not-an-email")]
    [TestCase("email@")]
    [TestCase("@test.com")]
    public async Task InvalidEmail_Returns400(string email)
    {
        var response = await Post(email);
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
    }

    [Test]
    public async Task AlreadyRegisteredEmail_Returns409()
    {
        var email = $"dup_{Guid.NewGuid():N}@test.com";
        await ClientCreatingFixture.ApiClient.Debug.AddUser(DebugUserFactory.Create(email: email));
        var response = await Post(email);
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.Conflict));
    }
}