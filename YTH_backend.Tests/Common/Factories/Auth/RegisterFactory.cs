using YTH_backend.DTOs.User;

namespace YTH_backend.Tests.Common.Factories.Auth;

public static class RegisterFactory
{
    public static CreateUserRequestDto Create(
        string? username = null,
        string? password = null
    )
        => new(
            username ?? ElementaryFactory.CreateUsername(),
            password ?? ElementaryFactory.CreatePassword()
        );
}