using Examples.Hosting.Workers;
using Examples.Hosting.Workers.Applications.Queueing;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((context, services) =>
    {
        services.AddHostedService<Worker>();

        services.AddQueueingApplications(context.Configuration);
    })
    .Build();

MonitorLoop monitorLoop = host.Services.GetRequiredService<MonitorLoop>()!;
monitorLoop.StartMonitorLoop();

await host.RunAsync();
