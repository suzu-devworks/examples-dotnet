using Microsoft.Extensions.Logging;

namespace Examples.Tests.Microsoft.Extensions.DependencyInjection.Fixtures.AmbiguousServices;

public sealed class UnresolvableParameterService
{
    public UnresolvableParameterService()
    {
    }

    public UnresolvableParameterService(ILogger<UnresolvableParameterService> logger)
    {
        Logger = logger;
    }

    public UnresolvableParameterService(FooService fooService, BarService barService)
    {
        Foo = fooService;
        Bar = barService;
    }

    public ILogger<UnresolvableParameterService>? Logger { get; }
    public FooService? Foo { get; }
    public BarService? Bar { get; }
}
