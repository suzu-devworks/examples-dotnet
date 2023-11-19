using Examples.Hosting.ScopedService;

namespace Examples.Hosting.Workers.Applications.ScopedService;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddScopedServiceApplications(this IServiceCollection services,
        IConfiguration _)
    {
        services.AddHostedService<ScopedBackgroundService>();
        services.AddScoped<IScopedProcessingService, DefaultScopedProcessingService>();

        return services;
    }
}
