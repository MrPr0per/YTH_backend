namespace YTH_backend.Tests.Common.Factories;

public static class ElementaryFactory
{
    public static string CreateGuid() => $"{Guid.NewGuid():N}";
    public static string CreateUsername() => $"user_{CreateGuid()}";
    public static string CreateEmail() => $"email_{CreateGuid()}@test.com";
    public static string CreatePassword() => $"password_{CreateGuid()}@test.com";
}