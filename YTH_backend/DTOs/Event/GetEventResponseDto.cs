using YTH_backend.Enums;

namespace YTH_backend.DTOs.Event;

public record GetEventResponseDto(Guid Id, string Name, string? Description, EventTypes Type, DateTime Date, string? Address, string? ImageUrl);



