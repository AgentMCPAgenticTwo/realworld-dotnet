using CleanArchitecture.Infrastructure.Common;
using CleanArchitecture.Infrastructure.Common.Middleware;

using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace CleanArchitecture.Infrastructure;

public static class RequestPipeline
{
    public static IApplicationBuilder UseInfrastructure(this IApplicationBuilder app)
    {
        app.UseMiddleware<EventualConsistencyMiddleware>();
        return app;
    }

    /// <summary>
    /// Brings the PostgreSQL database up to date with the latest migration on startup.
    /// Tests run against an in-memory provider that is created from the model instead.
    /// </summary>
    public static IApplicationBuilder ApplyMigrations(this IApplicationBuilder app)
    {
        using var scope = app.ApplicationServices.CreateScope();

        var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();

        if (dbContext.Database.IsNpgsql())
        {
            dbContext.Database.Migrate();
        }

        return app;
    }
}