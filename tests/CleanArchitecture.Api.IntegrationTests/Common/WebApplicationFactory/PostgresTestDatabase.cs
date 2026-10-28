using CleanArchitecture.Infrastructure.Common;

using Microsoft.EntityFrameworkCore;

using Testcontainers.PostgreSql;

namespace CleanArchitecture.Api.IntegrationTests.Common.WebApplicationFactory;

/// <summary>
/// Integration tests run against a real PostgreSQL database, spun up with Testcontainers,
/// so the migrations the API applies on startup are exercised as well.
/// </summary>
public class PostgresTestDatabase : IAsyncDisposable
{
    private readonly PostgreSqlContainer _container;

    public string ConnectionString => _container.GetConnectionString();

    public static async Task<PostgresTestDatabase> CreateAndInitializeAsync()
    {
        var testDatabase = new PostgresTestDatabase();

        await testDatabase._container.StartAsync();

        testDatabase.ResetDatabase();

        return testDatabase;
    }

    public void ResetDatabase()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseNpgsql(ConnectionString)
            .Options;

        using var context = new AppDbContext(options, null!, null!);
        context.Database.EnsureDeleted();
        context.Database.Migrate();
    }

    public async ValueTask DisposeAsync()
    {
        await _container.DisposeAsync();

        GC.SuppressFinalize(this);
    }

    private PostgresTestDatabase()
    {
        _container = new PostgreSqlBuilder()
            .WithDatabase("tutorialsdb")
            .WithUsername("postgres")
            .WithPassword("postgres")
            .Build();
    }
}
