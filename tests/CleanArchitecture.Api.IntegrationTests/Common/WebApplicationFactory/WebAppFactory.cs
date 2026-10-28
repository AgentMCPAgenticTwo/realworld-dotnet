using CleanArchitecture.Infrastructure.Common;

using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace CleanArchitecture.Api.IntegrationTests.Common.WebApplicationFactory;

public class WebAppFactory : WebApplicationFactory<IAssemblyMarker>, IAsyncLifetime
{
    private PostgresTestDatabase _testDatabase = null!;

    public AppHttpClient CreateAppHttpClient()
    {
        return new AppHttpClient(CreateClient());
    }

    public async Task InitializeAsync()
    {
        _testDatabase = await PostgresTestDatabase.CreateAndInitializeAsync();
    }

    public new async Task DisposeAsync()
    {
        await _testDatabase.DisposeAsync();
    }

    public void ResetDatabase()
    {
        _testDatabase.ResetDatabase();
    }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureTestServices(services => services
            .RemoveAll<DbContextOptions<AppDbContext>>()
            .AddDbContext<AppDbContext>((sp, options) => options.UseNpgsql(_testDatabase.ConnectionString)));

        builder.ConfigureAppConfiguration((context, conf) => conf.AddInMemoryCollection(new Dictionary<string, string?>
        {
            { "EmailSettings:EnableEmailNotifications", "false" },
        }));
    }
}
