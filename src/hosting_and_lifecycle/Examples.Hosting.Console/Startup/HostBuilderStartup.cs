using Examples.Hosting.Console.Applications;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Examples.Hosting.Console.Startup;

public class HostBuilderStartup
{
    public static async Task<int> RunAsync(string[] args, CancellationToken cancellationToken = default)
    {
        using IHost host = Host.CreateDefaultBuilder(args)
            .ConfigureServices(services =>
            {
                services.AddScoped<IRunner, HelloWorldRunner>();
            })
            .Build();

        using var scope = host.Services.CreateScope();

        var runner = scope.ServiceProvider.GetRequiredService<IRunner>();
        await runner.RunAsync("Hosting", cancellationToken);

        return 0;
    }
}
