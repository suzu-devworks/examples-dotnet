using Examples.Hosting.Workers;
using Examples.Hosting.Workers.Applications.QueueService;
using Examples.Hosting.Workers.Applications.ScopedService;
using Examples.Hosting.Workers.Applications.TimerService;

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
