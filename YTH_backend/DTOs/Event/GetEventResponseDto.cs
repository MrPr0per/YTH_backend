using YTH_backend.Enums;

namespace YTH_backend.DTOs.Event;

public record GetEventResponseDto(string Name, string? Description, EventTypes Type, DateTime Date, string? Address);



