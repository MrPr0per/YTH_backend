using YTH_backend.Enums;

namespace YTH_backend.DTOs.Post;


public record GetPostResponse(string Title, string Description, Status Status, DateTime CreatedAt);

public record CreatePostRequest(string Title, string Description, Status Status, DateTime CreatedAt);

public record PatchPostRequest(string? Title, string? Description, Status? Status, DateTime? CreatedAt);