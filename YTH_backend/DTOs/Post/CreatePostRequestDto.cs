using YTH_backend.Enums;

namespace YTH_backend.DTOs.Post;

public record CreatePostRequestDto(string Title, string Description, string? ImageBase64, PostStatus PostStatus = PostStatus.Posted);