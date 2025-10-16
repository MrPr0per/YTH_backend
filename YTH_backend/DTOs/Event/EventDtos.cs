using YTH_backend.Enums;

namespace YTH_backend.DTOs.Event;

public record GetEventsResponseDto(string Name, string? Description, EventTypes Type, DateTime Date, string? Address);

public record AddEventsRequestDto(string Name, string? Description, EventTypes Type, DateTime Date, string? Address);

public record PatchEventsRequestDto(string? Name, string? Description, EventTypes? Type, DateTime? Date, string? Address);