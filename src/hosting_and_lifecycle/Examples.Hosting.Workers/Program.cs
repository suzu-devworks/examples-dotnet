using Examples.Hosting.Applications.QueueService;
using Examples.Hosting.Applications.ScopedService;
using Examples.Hosting.Applications.TimerService;
using Examples.Hosting.Workers;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((context, services) =>
    {
        services.AddHostedService<Worker>();

        services.AddQueueServiceApplications(context.Configuration);
        services.AddScopedServiceApplications(context.Configuration);
        services.AddTimerServiceApplications(context.Configuration);
    })
    .Build();

MonitorLoop monitorLoop = host.Services.GetRequiredService<MonitorLoop>()!;
monitorLoop.StartMonitorLoop();

await host.RunAsync();
