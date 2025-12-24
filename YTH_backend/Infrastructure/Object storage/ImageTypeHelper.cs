namespace YTH_backend.Infrastructure.Object_storage;

public static class ImageTypeHelper
{
    public static string DetectImageContentType(byte[] bytes)
    {
        // JPEG
        if (bytes.Length >= 3 &&
            bytes[0] == 0xFF &&
            bytes[1] == 0xD8 &&
            bytes[2] == 0xFF)
            return "image/jpeg";

        // PNG
        if (bytes.Length >= 8 &&
            bytes[0] == 0x89 &&
            bytes[1] == 0x50 &&
            bytes[2] == 0x4E &&
            bytes[3] == 0x47)
            return "image/png";

        // GIF
        if (bytes.Length >= 6 &&
            bytes[0] == 0x47 &&
            bytes[1] == 0x49 &&
            bytes[2] == 0x46)
            return "image/gif";

        // WEBP
        if (bytes.Length >= 12 &&
            bytes[0] == 0x52 && // R
            bytes[1] == 0x49 && // I
            bytes[2] == 0x46 && // F
            bytes[3] == 0x46 &&
            bytes[8] == 0x57 && // W
            bytes[9] == 0x45 && // E
            bytes[10] == 0x42 && // B
            bytes[11] == 0x50)
            return "image/webp";

        throw new InvalidOperationException("Unsupported image format");
    }
    
    public static string? GetExtensionFromMimeType(string? mimeType)
    {
        return mimeType?.ToLowerInvariant() switch
        {
            "image/png" => ".png",
            "image/jpeg" or "image/jpg" => ".jpg",
            "image/gif" => ".gif",
            "image/bmp" => ".bmp",
            "image/webp" => ".webp",
            _ => null
        };
    }
}