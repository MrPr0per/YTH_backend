using NUnit.Framework;
using YTH_backend.Tests.Infrastructure;

namespace YTH_backend.Tests.Common.Assertions;

public static class ValidationAssertion
{
    public static async Task Run<T>(ValidationCase<T> validationCase, Func<T, Task<HttpResponseMessage>> run)
    {
        var content = validationCase.GetContent();
        var response = await run(content);
        var message = $"case: `{content}`, isValid: {validationCase.IsValid}, code: {response.StatusCode}";
        if (validationCase.IsValid)
            Assert.That(response.IsSuccessStatusCode, message);
        else
            Assert.That(response.StatusCode, Is.EqualTo(validationCase.ExpectedInvalidCode), message);
    }
}