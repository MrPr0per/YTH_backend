using System.Net;
using NUnit.Framework;
using YTH_backend.Tests.Common.Factories;
using YTH_backend.Tests.Infrastructure;

namespace YTH_backend.Tests.Common.ValidationCases;

public static class EmailValidationCases
{
    public static IEnumerable<TestCaseData> All =>
    [
        new(ValidationCase.New(false, () => "", HttpStatusCode.BadRequest)),
        new(ValidationCase.New(false, () => "ab", HttpStatusCode.BadRequest)),
        new(ValidationCase.New(false, () => "inv@lid", HttpStatusCode.BadRequest)),
        new(ValidationCase.New(false, () => "not-an-email", HttpStatusCode.BadRequest)),
        new(ValidationCase.New(false, () => "███好奇心█", HttpStatusCode.BadRequest)),
        new(ValidationCase.New(false, () => "email@", HttpStatusCode.BadRequest)),
        new(ValidationCase.New(false, () => "@test.com", HttpStatusCode.BadRequest)),

        new(ValidationCase.New(true, ElementaryFactory.CreateEmail, HttpStatusCode.BadRequest)),
    ];
}