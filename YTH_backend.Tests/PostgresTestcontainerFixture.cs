using NUnit.Framework;
using Testcontainers.PostgreSql;

namespace YTH_backend.Tests;

/// <summary>
/// Статический класс, создающий ApiFactory c тестовой бдшкой 
/// </summary>
[SetUpFixture]
public class PostgresTestcontainerFixture
{
    public static HttpClient Client { get; private set; } = null!;
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
        Client = Factory.CreateClient();
    }

    [OneTimeTearDown]
    public async Task GlobalTeardown()
    {
        Client.Dispose();
        await Factory.DisposeAsync();
        if (container != null)
            await container.DisposeAsync();
    }
}