using Microsoft.Extensions.Logging;

namespace Examples.Hosting.Console.Applications;

public class HelloWorldRunner : IRunner
{
    private readonly ILogger<HelloWorldRunner> _logger;

    public HelloWorldRunner(ILogger<HelloWorldRunner> logger)
    {
        _logger = logger;
    }

    public Task RunAsync(string[] args, CancellationToken cancelToken = default)
    {
        _logger.LogInformation("Hello world.");

        return Task.CompletedTask;
    }
}
