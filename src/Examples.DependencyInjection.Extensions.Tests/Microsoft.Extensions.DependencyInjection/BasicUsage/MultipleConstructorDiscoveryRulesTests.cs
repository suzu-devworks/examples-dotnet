using Examples.Tests.Microsoft.Extensions.DependencyInjection.Fixtures.AmbiguousServices;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Examples.Tests.Microsoft.Extensions.DependencyInjection.BasicUsage;

/// <summary>
/// When a type defines multiple constructors, 
/// the service provider learns which constructor to use.
/// </summary>
/// <seealso href="https://learn.microsoft.com/ja-jp/dotnet/core/extensions/dependency-injection#multiple-constructor-discovery-rules"/>
public class MultipleConstructorDiscoveryRulesTests
{
    [Fact]
    public void WhenConstructorHasUnResolvedParameters_UnresolvedConstructorNotUsed()
    {
        var services = new ServiceCollection()
            .AddLogging();

        services.AddSingleton<UnresolvableParameterService>();

        using var provider = services.BuildServiceProvider();

        var instance = provider.GetService<UnresolvableParameterService>();

        instance!.Logger.IsNotNull();   // ILogger<T> is resolved
        instance.Foo.IsNull();          // FooService is not resolved
        instance.Bar.IsNull();          // BarService is not resolved
    }


    [Fact]
    public void WhenConstructorHasAmbiguousParameters_ThrowsInvalidOperationException()
    {
        var services = new ServiceCollection()
            .AddLogging()
            .AddOptions();

        services.AddSingleton<AmbiguousParametersService>();

        using var provider = services.BuildServiceProvider();

        // It appears that `ActivatorUtilitiesConstructor` is not used in ServiceProvider.
        var ex = Assert.Throws<InvalidOperationException>(
            () => provider.GetService<AmbiguousParametersService>());
        ex.Message.Is(s => s.Contains("The following constructors are ambiguous:"));

    }


    [Fact]
    public void WhenConstructorHasAmbiguousParameters_WithImplementationFactory_ReturnsInstance()
    {
        var services = new ServiceCollection()
            .AddLogging()
            .AddOptions();

        services.AddSingleton(provider =>
            new AmbiguousParametersService(provider.GetRequiredService<ILogger<AmbiguousParametersService>>()));

        using var provider = services.BuildServiceProvider();

        var instance = provider.GetService<AmbiguousParametersService>();

        instance!.Logger.IsNotNull();   // ILogger<T> is resolved
        instance.Options.IsNull();      // IOptions<T> is not specified
    }

    [Fact]
    public void WhenConstructorHasAmbiguousParameters_WithActivatorUtilitiesConstructorAttribute_ReturnsInstance()
    {
        var services = new ServiceCollection()
            .AddLogging()
            .AddOptions();

        services.AddSingleton<AmbiguousParametersService>();

        using var provider = services.BuildServiceProvider();

        var instance = ActivatorUtilities.CreateInstance<AmbiguousParametersService>(provider);
        instance!.Logger.IsNull();          // ILogger<T> is not specified
        instance.Options.IsNotNull();       // IOptions<T> is resolved
    }


    [Fact]
    public void WhenConstructorDisambiguates_UseThatConstructor()
    {
        var services = new ServiceCollection()
            .AddLogging()
            .AddOptions();

        services.AddSingleton<DisambiguatesParametersService>();

        using var provider = services.BuildServiceProvider();

        var instance = provider.GetService<DisambiguatesParametersService>();

        instance!.Logger.IsNotNull();   // ILogger<T> is resolved
        instance.Options.IsNotNull();   // IOptions<T> is resolved
    }

}
