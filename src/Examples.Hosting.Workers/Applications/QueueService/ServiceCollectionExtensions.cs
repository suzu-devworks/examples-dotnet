using Examples.Hosting.Infrastructure.QueueService;

namespace Examples.Hosting.Applications.QueueService;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddQueueServiceApplications(this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddSingleton<MonitorLoop>();
        services.AddHostedService<QueuedHostedService>();
        services.AddSingleton<IBackgroundTaskQueue>(_ =>
        {
            var queueCapacity = configuration.GetValue<int?>("QueueCapacity") ?? 100;
            return new DefaultBackgroundTaskQueue(queueCapacity);
        });
        return services;
    }
}
