using System.Net.Http.Json;
using NUnit.Framework;

namespace YTH_backend.Tests.Infrastructure;

public static class AccessTokenReader // Читает ответ формата {access_token: str}
{
    public static async Task<string> Read(HttpResponseMessage response)
    {
        var responseJson = await response.Content.ReadFromJsonAsync<Dictionary<string, string>>();
        Assert.That(responseJson, Is.Not.Null);
        Assert.That(responseJson, Has.Count.EqualTo(1));
        Assert.That(responseJson, Contains.Key("access_token"));
        return responseJson!["access_token"];
    }
}