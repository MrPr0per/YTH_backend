using YTH_backend.Features.Debug;

namespace YTH_backend.Tests.Common.Factories.Debug;

public static class DebugUserFactory
{
    /// <param name="username">по умолчанию - user_{guid:N}</param>
    /// <param name="email">по умолчанию - email_{guid:N}@test.com</param>
    /// <param name="role"></param>
    public static AddUserDebugDto Valid(
        string? username = null,
        string? email = null,
        string role = "student"
    )
    {
        return new AddUserDebugDto(
            Username: username ?? $"user_{Guid.NewGuid():N}",
            Password: $"password_{Guid.NewGuid():N}",
            Email: email ?? $"email_{Guid.NewGuid():N}@test.com",
            Role: role
        );
    }
}