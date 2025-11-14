using YTH_backend.Enums;

namespace YTH_backend.Models.Infrastructure;

public record CursorParams(CursorType CursorType = CursorType.Default, Guid? CursorId = null);