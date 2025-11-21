namespace YTH_backend.DTOs.User;

public record GetNotificationsResponseDto(Guid Id, string Title, string Content, DateTime CreatedAt, bool IsRead, Guid UserId);