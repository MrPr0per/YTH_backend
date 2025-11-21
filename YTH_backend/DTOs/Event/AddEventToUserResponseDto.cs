namespace YTH_backend.DTOs.Event;

public record AddEventToUserResponseDto(Guid Id, DateTime CreatedAt, Guid UserId, Guid EventId);