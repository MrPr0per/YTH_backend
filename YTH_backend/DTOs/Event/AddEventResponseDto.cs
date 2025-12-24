using YTH_backend.Enums;

namespace YTH_backend.DTOs.Event;

public record AddEventResponseDto(Guid Id, string Name, string? Description, DateTime Date, EventTypes EventType, string? Address, string? ImageUrl);