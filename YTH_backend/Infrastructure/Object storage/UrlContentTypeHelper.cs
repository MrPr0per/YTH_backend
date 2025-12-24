namespace YTH_backend.Infrastructure.Object_storage;

public static class UrlContentTypeHelper
{
    public static string? TryGetContentTypeFromDataUri(string base64)
    {
        if (!base64.StartsWith("data:", StringComparison.OrdinalIgnoreCase))
            return null;

        var semicolonIndex = base64.IndexOf(';');
        if (semicolonIndex < 0)
            return null;
        
        return base64[5..semicolonIndex];
    }
}