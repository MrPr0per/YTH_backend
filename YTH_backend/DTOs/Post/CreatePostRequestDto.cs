using YTH_backend.Enums;

namespace YTH_backend.DTOs.Post;

public record CreatePostRequestDto(string Title, string Description, PostStatus PostStatus = PostStatus.Posted);