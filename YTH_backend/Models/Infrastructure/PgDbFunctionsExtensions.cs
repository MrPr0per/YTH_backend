using Microsoft.EntityFrameworkCore;

namespace YTH_backend.Models.Infrastructure;

public static class PgDbFunctionsExtensions
{
    // Обратите внимание: метод бросает исключение при попытке выполнить на клиенте.
    // Он используется ТОЛЬКО для построения SQL через HasDbFunction/HasTranslation.
    public static bool StringLessThan(this DbFunctions _, string? left, string? right)
        => throw new NotSupportedException($"{nameof(StringLessThan)} can only be used in LINQ-to-Entities queries.");

    public static bool StringGreaterThan(this DbFunctions _, string? left, string? right)
        => throw new NotSupportedException($"{nameof(StringGreaterThan)} can only be used in LINQ-to-Entities queries.");
}