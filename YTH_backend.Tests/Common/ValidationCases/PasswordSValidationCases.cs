using NUnit.Framework;
using YTH_backend.Tests.Common.Factories;
using YTH_backend.Tests.Infrastructure;

namespace YTH_backend.Tests.Common.ValidationCases;

public static class PasswordSValidationCases
{
    public static IEnumerable<TestCaseData> All =>
    [
        new(ValidationCase.New(false, () => "")),
        new(ValidationCase.New(false, () => "ab")),
        new(ValidationCase.New(false, () => "123")),
        new(ValidationCase.New(false, () => "password")),
        new(ValidationCase.New(false, () => "password123!A")),

        new(ValidationCase.New(true, () => ElementaryFactory.CreatePassword())),
        new(ValidationCase.New(true, () => "weoifoi34348gq348gq38akjasdlkKLJA")),
        new(ValidationCase.New(true, () => "ял\t\n统一码\r\v\u1234к███вм\\ом")),
        new(ValidationCase.New(true, () => "lk irlkkliifkbk fejfpbk qxosb gäqqää pxixfpfx " +
                                           "jrfpqffkmxklgx, glfqx hrhxxk bf hlphxxk irb")),
    ];
}