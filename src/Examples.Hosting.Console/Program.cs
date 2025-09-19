using System.CommandLine;
using Examples.Hosting.Console;
using Examples.Hosting.Console.Applications;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

var rootCommand = new RootCommand("Hosting example app")
    .AddCommand(new Command("used", "used Microsoft.Extensions.Hosting example."), async (_, cancelToken) =>
    {
        using IHost host = Host.CreateDefaultBuilder(args)
            .ConfigureServices(services => services.AddTransient<IRunner, HelloWorldRunner>())
            .Build();

        using var scope = host.Services.CreateScope();
        var runner = scope.ServiceProvider.GetRequiredService<IRunner>();
        await runner.RunAsync("Hosting", cancelToken);
    })
    .AddCommand(new Command("unused", "unused Microsoft.Extensions.Hosting example."), async (_, cancelToken) =>
    {
        var environmentName = Environment.GetEnvironmentVariable("DOTNET_ENVIRONMENT");
        IConfiguration configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: true, reloadOnChange: false)
            .AddJsonFile($"appsettings.{environmentName}.json", optional: true, reloadOnChange: false)
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
    });

await rootCommand.Parse(args).InvokeAsync();
