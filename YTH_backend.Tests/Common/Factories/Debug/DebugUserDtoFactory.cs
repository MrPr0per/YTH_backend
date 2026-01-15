using YTH_backend.Features.Debug;

namespace YTH_backend.Tests.Common.Factories.Debug;

public static class DebugUserDtoFactory
{
    public static AddUserDebugDto Create(
        string? username = null,
        string? password = null,
        string? email = null,
        string role = "student"
    )
    {
        return new AddUserDebugDto(
            username ?? ElementaryFactory.CreateUsername(),
            password ?? ElementaryFactory.CreatePassword(),
            email ?? ElementaryFactory.CreateEmail(),
            role
        );
    }
}