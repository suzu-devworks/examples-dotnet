namespace Examples.Hosting.Applications.TimerService;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddTimerServiceApplications(this IServiceCollection services,
        IConfiguration _)
    {
        services.AddHostedService<TimerHostedService>();

        return services;
    }

}
