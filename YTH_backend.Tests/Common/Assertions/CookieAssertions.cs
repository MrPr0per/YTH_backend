using NUnit.Framework;

namespace YTH_backend.Tests.Common.Assertions;

public static class CookieAssertions
{
    private const string RefreshTokenCookieName = "refreshToken";

    /// <summary>
    /// Полная проверка, что рефреш токен:
    /// - существует и не пустой
    /// - HttpOnly
    /// - Secure
    /// - SameSite=Strict
    /// </summary>
    public static void RefreshTokenIsValid(HttpResponseMessage response)
    {
        HasRefreshToken(response);

        RefreshTokenIsHttpOnlySecureStrict(response);

        var cookieValue = GetCookieValue(GetRefreshTokenCookie(response));

        Assert.That(
            cookieValue,
            Does.StartWith($"{RefreshTokenCookieName}="),
            "Refresh token cookie has invalid format"
        );

        Assert.That(
            cookieValue.Length,
            Is.GreaterThan($"{RefreshTokenCookieName}=".Length),
            "Refresh token cookie value is empty"
        );
    }

    public static void RefreshTokenChanged(HttpResponseMessage oldResponse, HttpResponseMessage newResponse)
    {
        var oldCookieValue = GetCookieValue(GetRefreshTokenCookie(oldResponse));
        var newCookieValue = GetCookieValue(GetRefreshTokenCookie(newResponse));
        Assert.That(
            oldCookieValue,
            Is.Not.EqualTo(newCookieValue),
            "Refresh token value was not rotated"
        );
    }

    public static void RefreshTokenCleared(HttpResponseMessage response)
    {
        var cookie = GetRefreshTokenCookie(response);

        Assert.That(
            cookie.Contains("Max-Age=0") || cookie.Contains("expires="),
            "Refresh token cookie must be cleared (Max-Age=0 or expired)"
        );
    }

    private static void HasRefreshToken(HttpResponseMessage response)
    {
        var cookies = GetSetCookieHeaders(response);
        Assert.That(
            cookies.Any(c => c.StartsWith($"{RefreshTokenCookieName}=")),
            $"Response does not contain '{RefreshTokenCookieName}' cookie"
        );
    }

    private static void RefreshTokenIsHttpOnlySecureStrict(HttpResponseMessage response)
    {
        var cookie = GetRefreshTokenCookie(response).ToLowerInvariant();

        Assert.That(cookie, Does.Contain("httponly"), "Refresh token cookie must be HttpOnly");
        Assert.That(cookie, Does.Contain("secure"), "Refresh token cookie must be Secure");
        Assert.That(cookie, Does.Contain("samesite=strict"), "Refresh token cookie must have SameSite=Strict");
    }

    private static IReadOnlyList<string> GetSetCookieHeaders(HttpResponseMessage response)
    {
        Assert.That(
            response.Headers.TryGetValues("Set-Cookie", out var values),
            "Response does not contain Set-Cookie headers"
        );

        return values!.ToList();
    }

    private static string GetRefreshTokenCookie(HttpResponseMessage response)
    {
        var cookies = GetSetCookieHeaders(response);

        var cookie = cookies.FirstOrDefault(
            c => c.StartsWith($"{RefreshTokenCookieName}=")
        );

        Assert.That(
            cookie,
            Is.Not.Null,
            $"'{RefreshTokenCookieName}' cookie not found"
        );

        return cookie!;
    }

    private static string GetCookieValue(string cookie)
    {
        // refresh_token=value; Path=/; HttpOnly; Secure; ...
        var endIndex = cookie.IndexOf(';');
        return endIndex == -1
            ? cookie
            : cookie.Substring(0, endIndex);
    }
}