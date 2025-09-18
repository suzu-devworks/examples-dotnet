using Examples.Tests.Microsoft.Extensions.DependencyInjection.Fixtures.Greetings;
using Microsoft.Extensions.DependencyInjection;

namespace Examples.Tests.Microsoft.Extensions.DependencyInjection.BasicUsage;

public partial class ServiceLifetimesTests
{
    [Fact]
    public void WhenRegisteredWithAddTransient_ReturnsDifferentInstanceEachTime()
    {
        var mock = new Mock<IMessagePrinter>();
        mock.Setup(x => x.Print("Hello Ioc world."));

        var services = new ServiceCollection();
        services.AddSingleton<IMessagePrinter>(_ => mock.Object);
        services.AddTransient<IMessageGenerator, HelloMessageGenerator>();
        services.AddTransient<IGreetingService, GreetingService>();

        using var provider = services.BuildServiceProvider();

        var instance = provider.GetService<IGreetingService>();
        instance?.Greet();

        var another = provider.GetService<IGreetingService>();
        another?.Greet();

        another!.IsNotSameReferenceAs(instance);

        mock.Verify(x => x.Print("Hello Ioc world."), Times.Exactly(2));
        mock.VerifyNoOtherCalls();
    }

    [Fact]
    public void WhenRegisteredWithAddScoped_ReturnsSameInstanceWithinScope()
    {
        var mock = new Mock<IMessagePrinter>();
        mock.Setup(x => x.Print("Hello Ioc world."));

        var services = new ServiceCollection();
        services.AddScoped<IMessagePrinter>(_ => mock.Object);
        services.AddScoped<IMessageGenerator, HelloMessageGenerator>();
        services.AddScoped<IGreetingService, GreetingService>();

        using var provider = services.BuildServiceProvider();

        var outScope = provider.GetService<IGreetingService>();
        outScope?.Greet();

        using (var scope = provider.CreateScope())
        {
            var inScope = scope.ServiceProvider.GetService<IGreetingService>();
            inScope?.Greet();

            inScope!.IsNotSameReferenceAs(outScope);

            var anotherInScope = scope.ServiceProvider.GetService<IGreetingService>();
            anotherInScope?.Greet();

            anotherInScope!.IsSameReferenceAs(inScope);
            anotherInScope!.IsNotSameReferenceAs(outScope);
        }

        mock.Verify(x => x.Print("Hello Ioc world."), Times.Exactly(3));
        mock.VerifyNoOtherCalls();
    }

    [Fact]
    public void WhenRegisteredWithAddSingleton_AlwaysReturnsSameInstance()
    {
        var mock = new Mock<IMessagePrinter>();
        mock.Setup(x => x.Print("Hello Ioc world."));

        var services = new ServiceCollection();
        services.AddSingleton<IMessagePrinter>(_ => mock.Object);
        services.AddSingleton<IMessageGenerator, HelloMessageGenerator>();
        services.AddSingleton<IGreetingService, GreetingService>();

        using var provider = services.BuildServiceProvider();

        var instance = provider.GetService<IGreetingService>();
        instance?.Greet();

        var another = provider.GetService<IGreetingService>();
        another?.Greet();

        another!.IsSameReferenceAs(instance);

        using (var scope = provider.CreateScope())
        {
            var inScope = scope.ServiceProvider.GetService<IGreetingService>();
            inScope!.Greet();

            inScope!.IsSameReferenceAs(instance);
        }

        mock.Verify(x => x.Print("Hello Ioc world."), Times.Exactly(3));
        mock.VerifyNoOtherCalls();
    }

}
