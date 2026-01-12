using Examples.Hosting.Console.Applications;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

static async Task RunWithHostingAsync(string[] args, CancellationToken cancelToken = default)
{
    using IHost host = Host.CreateDefaultBuilder(args)
        .ConfigureServices(services => services.AddTransient<IRunner, HelloWorldRunner>())
        .Build();

    using var scope = host.Services.CreateScope();
    var runner = scope.ServiceProvider.GetRequiredService<IRunner>();
    await runner.RunAsync("Hosting", cancelToken);
}

static async Task RunWithUnusedHostingAsync(string[] args, CancellationToken cancelToken = default)
{
    var environmentName = Environment.GetEnvironmentVariable("DOTNET_ENVIRONMENT");
    var configuration = new ConfigurationBuilder()
        .SetBasePath(Directory.GetCurrentDirectory())
        .AddJsonFile("appsettings.json", optional: true, reloadOnChange: false)
        .AddJsonFile($"appsettings.{environmentName}.json", optional: true, reloadOnChange: false)
        .AddUserSecrets<Program>(optional: true, reloadOnChange: false)
        .AddEnvironmentVariables()
        .AddCommandLine(args)
        .Build();

    using ServiceProvider provider = new ServiceCollection()
        .AddLogging(logging => logging
            .AddConfiguration(configuration.GetSection("Logging"))
            .AddConsole()
            .AddDebug()
            .AddEventSourceLogger())
        .AddSingleton<IConfiguration>(_ => configuration)
        .AddTransient<IRunner, HelloWorldRunner>()
        .BuildServiceProvider();

    using var scope = provider.CreateScope();
    var runner = scope.ServiceProvider.GetRequiredService<IRunner>();
    await runner.RunAsync("ServiceProvider", cancelToken);
}

using var cts = new CancellationTokenSource();
Console.CancelKeyPress += (s, e) =>
{
    Console.WriteLine("Cancellation request received. Starting graceful shutdown...");
    e.Cancel = true;
    cts.Cancel();
};

if (args.Contains("--unused", StringComparer.OrdinalIgnoreCase))
{
    await RunWithUnusedHostingAsync(args, cts.Token);
}
else
{
    await RunWithHostingAsync(args, cts.Token);
}
