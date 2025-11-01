using YTH_backend.Enums;

namespace YTH_backend.DTOs.Event;

public record PatchEventRequestDto(string? Name, string? Description, string? ShortDescription, EventTypes? Type, DateTime? Date, string? Address);