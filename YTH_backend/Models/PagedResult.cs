using YTH_backend.Enums;

namespace YTH_backend.Models;

public class PagedResult<T>
{
    public int From { get; init; }
    public int Take { get; init; }
    public OrderType OrderType { get; init; }
    public IEnumerable<T> Items { get; init;  } = Enumerable.Empty<T>();
}