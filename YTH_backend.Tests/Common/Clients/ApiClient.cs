namespace YTH_backend.Tests.Common.Clients;

public class ApiClient(HttpClient client)
{
    private const string ApiPrefix = "/api/v0";
    public AuthClient Auth { get; } = new(client, ApiPrefix, "/auth");
    public DebugClient Debug { get; } = new(client, ApiPrefix, "/debug");
}