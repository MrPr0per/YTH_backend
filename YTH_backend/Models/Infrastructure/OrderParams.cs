using YTH_backend.Enums;

namespace YTH_backend.Models.Infrastructure;

public record OrderParams(OrderType OrderType = OrderType.Asc, string FieldName = "Id");