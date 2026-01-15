using System.Net;

namespace YTH_backend.Tests.Infrastructure;

public readonly struct ValidationCase<T>(
    bool isValid,
    Func<T> getContent,
    HttpStatusCode expectedInvalidCode = HttpStatusCode.UnprocessableEntity
)
{
    public readonly bool IsValid = isValid;
    public readonly Func<T> GetContent = getContent;
    public readonly HttpStatusCode ExpectedInvalidCode = expectedInvalidCode;
}

public static class ValidationCase
    // отдельный статический класс для конструктора, чтобы тип T выводился автоматически
    // и можно было писать:  ValidationCase.New(false, "inv@lid")
    //              вместо:  new ValidationCase<string>(false, "inv@lid")
{
    public static ValidationCase<T> New<T>(
        bool isValid,
        Func<T> getContent,
        HttpStatusCode expectedInvalidCode = HttpStatusCode.UnprocessableEntity
    )
        => new(isValid, getContent, expectedInvalidCode);
}