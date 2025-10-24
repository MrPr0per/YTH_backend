using YTH_backend.Enums;

namespace YTH_backend.DTOs.User;

public record CreateUserRequestDto(string UserName, string Password, string Email);

//TODO разобраться с этим

//public record GetUserDto

//public record PatchUser()

