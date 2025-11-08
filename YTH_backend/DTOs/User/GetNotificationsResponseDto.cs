namespace YTH_backend.DTOs.User;

public record GetNotificationsResponseDto(string Title, string NotificationText, DateTime CreatedAt, bool IsRead);