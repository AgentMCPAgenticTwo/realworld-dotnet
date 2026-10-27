namespace CleanArchitecture.Api;

public static class DependencyInjection
{
    public const string AllowFrontendCorsPolicy = "AllowFrontend";

    public static IServiceCollection AddPresentation(this IServiceCollection services)
    {
        services.AddControllers();
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();
        services.AddProblemDetails();
        services.AddCors(options => options.AddPolicy(
            AllowFrontendCorsPolicy,
            policy => policy
                .WithOrigins("http://localhost:4200")
                .AllowAnyHeader()
                .AllowAnyMethod()));

        return services;
    }
}