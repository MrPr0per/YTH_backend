using YTH_backend.Enums;

namespace YTH_backend.DTOs.Post;

public record PatchPostRequestDto(string? Title, string? Description, PostStatus? Status, DateTime? CreatedAt, string? ImageBase64);