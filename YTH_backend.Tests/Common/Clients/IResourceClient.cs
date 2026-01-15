using System.Net.Http.Headers;
using System.Net.Http.Json;
using YTH_backend.Tests.Infrastructure;

namespace YTH_backend.Tests.Common.Clients;

public abstract class BaseResourceClient(HttpClient client, string apiPrefix, string resourcePrefix)
{
    public HttpClient Client { get; } = client;
    public UrlPath FullPrefix { get; } = new(apiPrefix, resourcePrefix);

    protected async Task<HttpResponseMessage> PostAsync(
        string route,
        object? body = null,
        string? accessToken = null
    )
    {
        var request = new HttpRequestMessage(
            HttpMethod.Post,
            FullPrefix / route
        );

        if (body is not null)
            request.Content = JsonContent.Create(body);

        if (accessToken is not null)
        {
            request.Headers.Authorization =
                new AuthenticationHeaderValue("Bearer", accessToken);
        }

        return await Client.SendAsync(request);
    }
}