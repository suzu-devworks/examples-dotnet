using Examples.DependencyInjection.IoC.Tests;
using Microsoft.Extensions.DependencyInjection;

namespace Examples.DependencyInjection.Tests;

/// <summary>
/// Tests to study dependency injection using <see cref="Microsoft.Extensions.DependencyInjection" />.
/// </summary>
public partial class ServiceProviderTests
{

    [Fact]
    public void WhenSimpleInjection_WithTransient()
    {
        var mock = new Mock<IMessagePrinter>();
        mock.Setup(x => x.Print("Hello DI world."));

        var services = new ServiceCollection();
        services.AddTransient<IMessagePrinter>(_ => mock.Object);
        services.AddTransient<IMessageGenerator, MyMessageGenerator>();
        services.AddTransient<IMyService, MyService>();

        using var provider = services.BuildServiceProvider();

        var service = provider.GetService<IMyService>();
        service!.Greet();

        var other = provider.GetService<IMyService>();
        other!.Greet();

        // If you register with `AddTransient()`,
        //  `GetService()` will return a new instance every time.
        object.ReferenceEquals(service, other).IsFalse();

        mock.Verify(x => x.Print("Hello DI world."), Times.Exactly(2));
        mock.VerifyNoOtherCalls();
        return;
    }

    [Fact]
    public void WhenSimpleInjection_WithScoped()
    {
        var mock = new Mock<IMessagePrinter>();
        mock.Setup(x => x.Print("Hello DI world."));

        var services = new ServiceCollection();
        services.AddScoped<IMessagePrinter>(_ => mock.Object);
        services.AddScoped<IMessageGenerator, MyMessageGenerator>();
        services.AddScoped<IMyService, MyService>();

        using var provider = services.BuildServiceProvider();

        IMyService? service1 = null;
        using (var scope = provider.CreateScope())
        {
            var service = scope.ServiceProvider.GetService<IMyService>();
            service!.Greet();

            var other = scope.ServiceProvider.GetService<IMyService>();
            other!.Greet();

            // Instances in the same scope are the same.
            object.ReferenceEquals(service, other).IsTrue();

            // Survive without being garbage.
            service1 = service;
        }

        IMyService? service2 = null;
        using (var scope = provider.CreateScope())
        {
            var service = scope.ServiceProvider.GetService<IMyService>();
            service!.Greet();

            var other = scope.ServiceProvider.GetService<IMyService>();
            other!.Greet();

            // Instances in the same scope are the same.
            object.ReferenceEquals(service, other).IsTrue();

            // Survive without being garbage.
            service2 = service;
        }

        // If the scope is different, the instance is also different.
        object.ReferenceEquals(service1, service2).IsFalse();

        mock.Verify(x => x.Print("Hello DI world."), Times.Exactly(4));
        mock.VerifyNoOtherCalls();
        return;
    }

    [Fact]
    public void WhenSimpleInjection_WithSingleton()
    {
        var mock = new Mock<IMessagePrinter>();
        mock.Setup(x => x.Print("Hello DI world."));

        var services = new ServiceCollection();
        services.AddSingleton<IMessagePrinter>(_ => mock.Object);
        services.AddSingleton<IMessageGenerator, MyMessageGenerator>();
        services.AddSingleton<IMyService, MyService>();

        using var provider = services.BuildServiceProvider();

        IMyService? service1 = null;
        using (var scope = provider.CreateScope())
        {
            var service = scope.ServiceProvider.GetService<IMyService>();
            service!.Greet();

            // Survive without being garbage.
            service1 = service;
        }

        IMyService? service2 = null;
        using (var scope = provider.CreateScope())
        {
            var service = scope.ServiceProvider.GetService<IMyService>();
            service!.Greet();

            // Survive without being garbage.
            service2 = service;
        }

        // Always the same instance.
        object.ReferenceEquals(service1, service2).IsTrue();

        mock.Verify(x => x.Print("Hello DI world."), Times.Exactly(2));
        mock.VerifyNoOtherCalls();
        return;
    }

    [Fact]
    public void WhenMultipleInjection()
    {
        var mock = new Mock<IMessagePrinter>();
        mock.Setup(x => x.Print("Hello DI world."));
        mock.Setup(x => x.Print("Hello DI world 2nd."));
        mock.Setup(x => x.Print("Hello DI world 3rd."));

        var services = new ServiceCollection();
        services.AddSingleton<IMessagePrinter>(_ => mock.Object);
        services.AddSingleton<IMessageGenerator, MyMessageGenerator>();
        services.AddSingleton<IMessageGenerator, MyMessageGenerator2>();
        services.AddSingleton<IMessageGenerator, MyMessageGenerator3>();
        services.AddTransient<IMyService, MyService>();

        using var provider = services.BuildServiceProvider();

        // Use.
        var service = provider.GetService<IMyService>();
        service?.Greet();

        var other = provider.GetService<IMyService>();
        other?.Greet();

        // The instance is also different.
        object.ReferenceEquals(service, other).IsFalse();

        mock.Verify(x => x.Print("Hello DI world."), Times.Exactly(2));
        mock.Verify(x => x.Print("Hello DI world 2nd."), Times.Exactly(2));
        mock.Verify(x => x.Print("Hello DI world 3rd."), Times.Exactly(2));
        mock.VerifyNoOtherCalls();
        return;
    }


}
