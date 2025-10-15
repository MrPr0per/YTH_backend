using YTH_backend.Enums;

namespace YTH_backend.DTOs.Event;

public record GetEventsResponse(string Name, string? Description, EventTypes Type, DateTime Date, string? Address);

public record AddEventsRequest(string Name, string? Description, EventTypes Type, DateTime Date, string? Address);