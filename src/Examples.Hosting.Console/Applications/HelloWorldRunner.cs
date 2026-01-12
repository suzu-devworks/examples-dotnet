using Microsoft.Extensions.Logging;

namespace Examples.Hosting.Console.Applications;

public class HelloWorldRunner(ILogger<HelloWorldRunner> logger) : IRunner
{
    private readonly ILogger<HelloWorldRunner> _logger = logger;

    public Task RunAsync(string param, CancellationToken cancelToken = default)
    {
        _logger.LogInformation("Hello {param}.", param);

        return Task.CompletedTask;
    }
}
