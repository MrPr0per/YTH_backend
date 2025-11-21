namespace YTH_backend.DTOs.Event;

public record GetUserEventByIdResponseDto(Guid Id, DateTime CreatedAt, Guid UserId, Guid EventId);