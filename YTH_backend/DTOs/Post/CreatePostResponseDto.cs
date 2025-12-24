using YTH_backend.Enums;

namespace YTH_backend.DTOs.Post;

public record CreatePostResponseDto(Guid Id, Guid AuthorId, string Title, string Description, PostStatus PostStatus, DateTime CreatedAt, string? ImageUrl);