namespace YTH_backend.DTOs.Event;

public record AddEventToUserRequestDto(Guid EventId, Guid UserId);