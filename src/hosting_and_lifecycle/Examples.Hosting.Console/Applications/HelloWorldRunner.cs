using Microsoft.Extensions.Logging;

namespace Examples.Hosting.Console.Applications;

public class HelloWorldRunner(ILogger<HelloWorldRunner> logger) : IRunner
{
    public async Task RunAsync(string param, CancellationToken cancellationToken = default)
    {
        if (logger.IsEnabled(LogLevel.Information))
        {
            logger.LogInformation("Hello {param}.", param);
        }

        await Countdown.From(10).DoAsync(count =>
            {
                if (logger.IsEnabled(LogLevel.Information))
                {
                    logger.LogInformation("Shutting down in {count} seconds...", count);
                }
            }, cancellationToken);
    }
}
