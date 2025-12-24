using YTH_backend.Enums;

namespace YTH_backend.DTOs.Post;

public record GetPostResponseDto(Guid PostId, Guid AuthorId, string Title, string Description, PostStatus PostStatus, DateTime CreatedAt, string? ImageUrl);



