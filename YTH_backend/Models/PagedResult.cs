using YTH_backend.Enums;

namespace YTH_backend.Models;

public class PagedResult<T>(int from, int take, OrderType orderType, List<T> items)
{
    public int From { get; init; } = from;
    public int Take { get; init; } = take;
    public OrderType OrderType { get; init; } = orderType;
    public List<T> Items { get; init; } = items;
    public int ActualTake => Items.Count;
}