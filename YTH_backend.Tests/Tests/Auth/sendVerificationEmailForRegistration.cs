using System.Net;
using NUnit.Framework;
using YTH_backend.DTOs.User;
using YTH_backend.Tests.Common.Assertions;
using YTH_backend.Tests.Common.Factories.Debug;
using YTH_backend.Tests.Common.ValidationCases;
using YTH_backend.Tests.Infrastructure;

namespace YTH_backend.Tests.Tests.Auth;

[TestFixture]
public class SendVerificationEmailForRegistrationTests
{
    private static Task<HttpResponseMessage> Post(string email) =>
        ClientCreatingFixture.ApiClient.Auth.SendVerificationEmailForRegistration(
            new SendVerificationEmailRequestDto(email)
        );

    [TestCaseSource(typeof(EmailValidationCases), nameof(EmailValidationCases.All))]
    public async Task EmailValidationTests(ValidationCase<string> validationCase) =>
        await ValidationAssertion.Run(validationCase, Post);


    [Test]
    public async Task AlreadyRegisteredEmail_Returns409()
    {
        var email = $"dup_{Guid.NewGuid():N}@test.com";
        await ClientCreatingFixture.ApiClient.Debug.AddUser(DebugUserDtoFactory.Create(email: email));
        var response = await Post(email);
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.Conflict));
    }
}