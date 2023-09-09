using Examples.Hosting.Queueing;

namespace Examples.Hosting.Workers.Applications.Queueing;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddQueueingApplications(this IServiceCollection services,
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
