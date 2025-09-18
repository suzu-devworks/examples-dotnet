using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Examples.Tests.Microsoft.Extensions.DependencyInjection.Fixtures.AmbiguousServices;

public sealed class DisambiguatesParametersService
{
    public DisambiguatesParametersService()
    {
    }

    public DisambiguatesParametersService(
        ILogger<DisambiguatesParametersService> logger,
        IOptions<ExampleOptions> options)
    {
        Logger = logger;
        Options = options.Value;
    }

    public ILogger<DisambiguatesParametersService>? Logger { get; }

    public ExampleOptions? Options { get; }
}
