using YTH_backend.Enums;
using YTH_backend.Models.Infrastructure;

namespace YTH_backend.Infrastructure;

public static class QueryParamsParser
{
    public static CursorParams ParseCursorParams(string? query)
    {
        if (query == null)
            return new CursorParams();
        
        var split = query.Split(':');
        
        if (split.Length != 2)
            throw new ArgumentException("Cursor query must be in format '<type>:<id>'", nameof(query));
        
        if (!Guid.TryParse(split[1], out var id))
            throw new ArgumentException("Cursor query must have a valid GUID", nameof(query));
            
        if (!Enum.TryParse<CursorType>(split[0], true, out var cursorType))
            throw new ArgumentException("Invalid cursor type", nameof(query));
        
        return new CursorParams(cursorType, id);
    }

    public static OrderParams ParseOrderParams(string? query)
    {
        if (query == null)
            return new OrderParams();
        
        var split = query.Split(':');
        
        if (split.Length != 2)
            throw new ArgumentException("Order query must be in format '<type>:<fieldName>'", nameof(query));
        
        var fieldName = split[1];
        
        if (string.IsNullOrWhiteSpace(fieldName))
            throw new ArgumentException("Field name must not be empty", nameof(query));
        
        if (!Enum.TryParse<OrderType>(split[0], true, out var orderType))
            throw new ArgumentException("Invalid order type", nameof(query));
        
        return new OrderParams(orderType, fieldName);
    }
}