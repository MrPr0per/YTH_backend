using NUnit.Framework;
using Testcontainers.PostgreSql;
using YTH_backend.Tests.Common.Clients;

namespace YTH_backend.Tests.Infrastructure;

/// <summary>
/// Статический класс, создающий клиента для апишки c тестовой бдшкой 
/// </summary>
[SetUpFixture]
public class ClientCreatingFixture
{
    public static ApiClient ApiClient { get; private set; } = null!;
    public static HttpClient HttpClient { get; private set; } = null!;
    public static ApiFactory Factory { get; private set; } = null!;
    private static PostgreSqlContainer? container;

    [OneTimeSetUp]
    public async Task GlobalSetup()
    {
        // Создаём контейнер PostgreSQL
        container = new PostgreSqlBuilder()
            .WithDatabase("yth_test")
            .WithUsername("test")
            .WithPassword("test")
            .WithImage("postgres:15")
            .Build();
        await container.StartAsync();

        Factory = new ApiFactory(container.GetConnectionString());
        HttpClient = Factory.CreateClient();
        ApiClient = new ApiClient(HttpClient);
    }

    [OneTimeTearDown]
    public async Task GlobalTeardown()
    {
        HttpClient.Dispose();
        await Factory.DisposeAsync();
        if (container != null)
            await container.DisposeAsync();
    }
}