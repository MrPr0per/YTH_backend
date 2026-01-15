using YTH_backend.Features.Debug;

namespace YTH_backend.Tests.Common.Factories.Debug;

public static class DebugUserFactory
{
    /// <param name="username">по умолчанию - user_{guid:N}</param>
    /// <param name="email">по умолчанию - email_{guid:N}@test.com</param>
    /// <param name="role"></param>
    public static AddUserDebugDto Create(
        string? username = null,
        string? email = null,
        string role = "student"
    )
    {
        return new AddUserDebugDto(
            Username: username ?? ElementaryFactory.CreateUsername(),
            Password: ElementaryFactory.CreatePassword(),
            Email: email ?? ElementaryFactory.CreateEmail(),
            Role: role
        );
    }
}