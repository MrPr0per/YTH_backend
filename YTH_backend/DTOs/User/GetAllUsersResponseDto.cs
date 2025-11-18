using StackExchange.Redis;
using YTH_backend.Enums;

namespace YTH_backend.DTOs.User;

public record GetAllUsersResponseDto(Guid Id, string UserName, string Email, Roles Role);