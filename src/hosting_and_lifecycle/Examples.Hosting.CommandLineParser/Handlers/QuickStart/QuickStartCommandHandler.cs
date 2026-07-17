using Examples.Hosting.CommandLineParser.Commands.QuickStart;
using Microsoft.Extensions.Logging;

namespace Examples.Hosting.CommandLineParser.Handlers.QuickStart;

public class QuickStartCommandHandler(
    QuickStartCommand options,
    ILogger<QuickStartCommandHandler> logger
) : ICommandHandler
{
    private readonly QuickStartCommand _options = options;
    private readonly ILogger<QuickStartCommandHandler> _logger = logger;
    private readonly TextWriter _writer = System.Console.Out;

    public async Task ExecuteAsync(CancellationToken cancellationToken = default)
    {
        _logger.LogTrace("Starting QuickStartCommandHandler with options: {@Options}", _options);

        await Task.Delay(2000, cancellationToken); // Simulate async work

        //handle options
        _writer.WriteLine("InputFiles: {0}", string.Join(", ", _options.InputFiles));
        _writer.WriteLine("Verbose: {0}", _options.Verbose);
        _writer.WriteLine("Stdin: {0}", _options.Stdin);
        _writer.WriteLine("Offset: {0}", _options.Offset);

        _logger.LogTrace("Finished processing QuickStartCommandHandler");
    }
}
