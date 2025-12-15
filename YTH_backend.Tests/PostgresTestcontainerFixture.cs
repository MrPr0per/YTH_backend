using NUnit.Framework;
using Testcontainers.PostgreSql;

namespace YTH_backend.Tests;

[SetUpFixture]
public class PostgresTestcontainerFixture
{
    // Статический контейнер, доступный из тестов
    public static PostgreSqlContainer? Container { get; private set; }
    public static ApiFactory? Factory { get; private set; }

    [OneTimeSetUp]
    public async Task GlobalSetup()
    {
        // Создаём контейнер PostgreSQL
        Container = new PostgreSqlBuilder()
            .WithDatabase("yth_test")
            .WithUsername("test")
            .WithPassword("test")
            .WithImage("postgres:18") // todo: хз какая версия будет на проде, поставил последнюю, но не :latest для стабильности
            .Build();
        await Container.StartAsync();

        Factory = new ApiFactory(Container.GetConnectionString());
    }

    [OneTimeTearDown]
    public async Task GlobalTeardown()
    {
        if (Container != null)
            await Container.DisposeAsync();
        if (Factory != null)
            await Factory.DisposeAsync();
    }
}