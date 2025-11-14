using YTH_backend.Enums;

namespace YTH_backend.Models.Infrastructure;

public record PagedResult<T>(int Take, string OrderFieldName, OrderType OrderType, CursorType CursorType, List<T>? Items)
{
    public int ActualTake => Items?.Count ?? 0;
}