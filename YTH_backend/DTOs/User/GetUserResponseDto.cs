using YTH_backend.Enums;

namespace YTH_backend.DTOs.User;

public record GetUserResponseDto(string UserName, Roles Role);