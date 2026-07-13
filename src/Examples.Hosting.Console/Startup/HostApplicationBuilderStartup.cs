using Examples.Hosting.Console.Applications;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Examples.Hosting.Console.Startup;

public static class HostApplicationBuilderStartup
{
    public static async Task<int> RunAsync(string[] args, CancellationToken cancellationToken = default)
    {
        var builder = Host.CreateApplicationBuilder(args);

        builder.Services.AddScoped<IRunner, HelloWorldRunner>();

        using IHost host = builder.Build();

        using var scope = host.Services.CreateScope();
        var runner = scope.ServiceProvider.GetRequiredService<IRunner>();
        await runner.RunAsync("Host Application", cancellationToken);

        return 0;
    }

    public static async Task<int> RunWithHostedAsync(string[] args)
    {
        var builder = Host.CreateApplicationBuilder(args);

        builder.Services.AddSingleton<IHostLifetime, CustomConsoleLifeTime>();
        builder.Services.AddHostedService<HelloHostedServiceRunner>();

        using IHost host = builder.Build();

        await host.RunAsync();

        return 0;
    }
}
