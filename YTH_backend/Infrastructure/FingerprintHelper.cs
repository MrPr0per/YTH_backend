using System.Net;
using System.Net.Sockets;
using System.Security.Cryptography;
using System.Text;

namespace YTH_backend.Infrastructure;

public static class FingerprintHelper
{
    public static string GetFingerprint(HttpContext context, string? appSalt = null)
{
    // 1) Получаем IP с учётом доверенных прокси
    var ip = GetClientIpAddress(context) ?? "0.0.0.0";
    var maskedIp = MaskIpStable(ip);

    // 2) User-Agent -> нормализованный core
    var ua = context.Request.Headers["User-Agent"].ToString();
    var uaCore = NormalizeUserAgent(ua);

    // 3) Accept-Language — только основной тег
    var lang = context.Request.Headers["Accept-Language"].ToString()
        .Split(',', StringSplitOptions.RemoveEmptyEntries)
        .Select(s => s.Split(';')[0].Trim())
        .FirstOrDefault() ?? "unknown";

    // 4) Собираем строку (salt опционален)
    var raw = $"{maskedIp}|{uaCore}|{lang}";
    if (!string.IsNullOrEmpty(appSalt))
        raw = $"{raw}|{appSalt}";

    // 5) SHA-256 хеш (hex lowercase)
    var hash = SHA256.HashData(Encoding.UTF8.GetBytes(raw));
    return Convert.ToHexString(hash).ToLowerInvariant();
}

private static string? GetClientIpAddress(HttpContext context)
{
    // Если у тебя есть доверенные прокси, ты можешь читать X-Forwarded-For.
    // В противном случае — использовать RemoteIpAddress.
    // Важно: не доверять X-Forwarded-For без настройки доверенных прокси.
    var forwardedFor = context.Request.Headers["X-Forwarded-For"].FirstOrDefault();
    if (!string.IsNullOrWhiteSpace(forwardedFor))
    {
        // X-Forwarded-For может содержать список IP: "client, proxy1, proxy2"
        var first = forwardedFor.Split(',', StringSplitOptions.RemoveEmptyEntries).First().Trim();
        if (IPAddress.TryParse(first, out _))
            return first;
    }

    var remote = context.Connection.RemoteIpAddress;
    return remote?.ToString();
}

private static string MaskIpStable(string ip)
{
    if (!IPAddress.TryParse(ip, out var addr))
        return ip;

    if (addr.AddressFamily == AddressFamily.InterNetwork) // IPv4
    {
        var parts = ip.Split('.');
        if (parts.Length == 4)
            return $"{parts[0]}.{parts[1]}.{parts[2]}.0"; // /24
        return ip;
    }

    if (addr.AddressFamily == AddressFamily.InterNetworkV6) // IPv6
    {
        var bytes = addr.GetAddressBytes();
        // обнулим последние 80 бит, т.е. оставим первые 48 бит (6 байт)
        for (int i = 6; i < bytes.Length; i++)
            bytes[i] = 0;
        return new IPAddress(bytes).ToString();
    }

    return ip;
}

private static string NormalizeUserAgent(string ua)
{
    if (string.IsNullOrWhiteSpace(ua))
        return "unknown";
    
    try
    {
        ua = ua.ToLowerInvariant();

        string browser = "other";
        string version = "0";
        if (ua.Contains("chrome") && !ua.Contains("edg"))
        {
            browser = "chrome";
            version = ExtractMajorVersion(ua, "chrome/");
        }
        else if (ua.Contains("edg"))
        {
            browser = "edge";
            version = ExtractMajorVersion(ua, "edg/");
        }
        else if (ua.Contains("firefox"))
        {
            browser = "firefox";
            version = ExtractMajorVersion(ua, "firefox/");
        }
        else if (ua.Contains("safari") && ua.Contains("version/"))
        {
            browser = "safari";
            version = ExtractMajorVersion(ua, "version/");
        }

        var device = ua.Contains("mobile") || ua.Contains("android") || ua.Contains("iphone") ? "mobile" : "desktop";

        return $"{browser}-{version}-{device}";
    }
    catch
    {
        return "unknown";
    }
}

private static string ExtractMajorVersion(string ua, string token)
{
    var idx = ua.IndexOf(token, StringComparison.OrdinalIgnoreCase);
    if (idx < 0) return "0";
    idx += token.Length;
    var sb = new StringBuilder();
    while (idx < ua.Length && (char.IsDigit(ua[idx]) || ua[idx] == '.'))
    {
        sb.Append(ua[idx]);
        idx++;
    }
    var ver = sb.ToString().Split('.')[0];
    return string.IsNullOrEmpty(ver) ? "0" : ver;
}
}