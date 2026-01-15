using YTH_backend.Features.Debug;

namespace YTH_backend.Tests.Common.Clients;

public class DebugClient(HttpClient client, string apiPrefix, string resourcePrefix)
    : BaseResourceClient(client, apiPrefix, resourcePrefix)
{
    public Task<HttpResponseMessage> AddUser(AddUserDebugDto args) =>
        PostAsync("addUser", args);

    public Task<HttpResponseMessage> GetTokenWithConfirmedEmail(GetTokenWithConfirmedEmailDto args) =>
        PostAsync("getTokenWithConfirmedEmail", args);
}