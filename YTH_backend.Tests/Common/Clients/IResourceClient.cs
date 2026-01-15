using System.Net.Http.Json;
using YTH_backend.Tests.Infrastructure;

namespace YTH_backend.Tests.Common.Clients;

public abstract class BaseResourceClient(HttpClient client, string apiPrefix, string resourcePrefix)
{
    public HttpClient Client { get; } = client;
    public UrlPath FullPrefix { get; } = new(apiPrefix, resourcePrefix);

    protected Task<HttpResponseMessage> PostAsync(string route, object? body = null) =>
        Client.PostAsJsonAsync(FullPrefix / route, body);
}