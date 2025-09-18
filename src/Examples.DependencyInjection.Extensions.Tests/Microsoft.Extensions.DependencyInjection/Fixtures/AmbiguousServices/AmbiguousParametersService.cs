using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Examples.Tests.Microsoft.Extensions.DependencyInjection.Fixtures.AmbiguousServices;

public sealed class AmbiguousParametersService
{
    public AmbiguousParametersService()
    {
    }

    public AmbiguousParametersService(ILogger<AmbiguousParametersService> logger)
    {
        Logger = logger;
    }

    [ActivatorUtilitiesConstructor]
    public AmbiguousParametersService(IOptions<ExampleOptions> options)
    {
        Options = options.Value;
    }

    public ILogger<AmbiguousParametersService>? Logger { get; }

    public ExampleOptions? Options { get; }
}
