using YTH_backend.Enums;

namespace YTH_backend.DTOs.Post;

public record GetPostResponseDto(Guid AuthorId, string Title, string ShortDescription, string Description, Status Status, DateTime CreatedAt);



