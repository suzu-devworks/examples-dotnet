using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Examples.Hosting.Console.Applications;

public class HelloHostedServiceRunner(
    IHostApplicationLifetime appLifetime,
    ILogger<HelloHostedServiceRunner> logger)
    : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            if (logger.IsEnabled(LogLevel.Information))
            {
                logger.LogInformation("Hello {param}.", nameof(HelloHostedServiceRunner));
            }

            await Countdown.From(10).DoAsync(count =>
                {
                    if (logger.IsEnabled(LogLevel.Information))
                    {
                        logger.LogInformation("Shutting down in {count} seconds...", count);
                    }
                }, cancellationToken);
        }
        catch (OperationCanceledException) when (cancellationToken.IsCancellationRequested)
        {
            // normal shutdown
        }
        finally
        {
            // Request shutdown to trigger the application stopping events and allow for graceful shutdown.
            appLifetime.StopApplication();
        }
    }
}
