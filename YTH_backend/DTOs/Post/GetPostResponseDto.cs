using YTH_backend.Enums;

namespace YTH_backend.DTOs.Post;

public record GetPostResponseDto(string Title, string Description, Status Status, DateTime CreatedAt);



