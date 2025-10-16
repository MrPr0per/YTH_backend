using YTH_backend.Enums;

namespace YTH_backend.DTOs.User;

public record CreateUserRequestDto(string UserName, string Password);

//TODO разобраться с этим

//public record GetUserDto

//public record PatchUser()

public record AuthUserDtoRequestDto(string Login, string Password);

public record SendVerificationEmailDtoRequest(string Email);

public record UserChangePasswordDtoRequest(string OldPassword, string NewPassword);

public record UserResetPasswordDtoRequest(string NewPassword);