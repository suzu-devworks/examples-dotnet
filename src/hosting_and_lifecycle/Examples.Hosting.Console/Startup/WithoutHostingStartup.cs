using Examples.Hosting.Console.Applications;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Examples.Hosting.Console.Startup;

public class WithoutHostingStartup
{
    public static async Task<int> RunAsync(string[] args, CancellationToken cancellationToken = default)
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

        var services = new ServiceCollection();

        services.AddLogging(logging => logging
                .AddConfiguration(configuration.GetSection("Logging"))
                .AddConsole()
                .AddDebug()
                .AddEventSourceLogger());

        services.AddSingleton<IConfiguration>(_ => configuration);
        services.AddScoped<IRunner, HelloWorldRunner>();

        using ServiceProvider provider = services.BuildServiceProvider();

        using var scope = provider.CreateScope();

        var runner = scope.ServiceProvider.GetRequiredService<IRunner>();
        await runner.RunAsync("Without Host", cancellationToken);

        return 0;
    }
}
