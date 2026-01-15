using NUnit.Framework;
using YTH_backend.Tests.Common.Factories;
using YTH_backend.Tests.Infrastructure;

namespace YTH_backend.Tests.Common.ValidationCases;

public static class UsernameValidationCases
{
    public static IEnumerable<TestCaseData> All =>
    [
        new(ValidationCase.New(false, () => "")),
        new(ValidationCase.New(false, () => "ab")),
        new(ValidationCase.New(false, () => "inv@lid")),
        new(ValidationCase.New(false, () => "███好奇心█\u1234")),

        new(ValidationCase.New(true, () => $"username_ABC123_{ElementaryFactory.CreateGuid()}")),
        new(ValidationCase.New(true, ElementaryFactory.CreateUsername)),
    ];
}