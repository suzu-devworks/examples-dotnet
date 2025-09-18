using Autofac;
using Examples.Tests.Autofac.Fixtures.Greetings;

namespace Examples.Tests.Autofac.BasicUsage;

public partial class ServiceLifetimesTests
{
    [Fact]
    public void WhenRegisteredWithInstancePerDependency_ReturnsDifferentInstanceEachTime()
    {
        var mock = new Mock<IMessagePrinter>();
        mock.Setup(x => x.Print("Hello Ioc world."));

        var builder = new ContainerBuilder();

        builder.RegisterInstance(mock.Object)
            .As<IMessagePrinter>()
            .ExternallyOwned();
        builder.RegisterType<HelloMessageGenerator>()
            .As<IMessageGenerator>()
            //.InstancePerDependency() // default
            ;
        builder.RegisterType<GreetingService>()
            .As<IGreetingService>()
            //.InstancePerDependency() // default
            ;

        using var container = builder.Build();

        var instance = container.Resolve<IGreetingService>();
        instance?.Greet();

        var another = container.Resolve<IGreetingService>();
        another?.Greet();

        another!.IsNotSameReferenceAs(instance);

        mock.Verify(x => x.Print("Hello Ioc world."), Times.Exactly(2));
        mock.VerifyNoOtherCalls();
    }

    [Fact]
    public void WhenRegisteredWithInstancePerLifetimeScope_ReturnsSameInstanceWithinScope()
    {
        var mock = new Mock<IMessagePrinter>();
        mock.Setup(x => x.Print("Hello Ioc world."));

        var builder = new ContainerBuilder();

        builder.RegisterInstance(mock.Object)
            .As<IMessagePrinter>()
            .ExternallyOwned();
        builder.RegisterType<HelloMessageGenerator>()
            .As<IMessageGenerator>()
            .InstancePerLifetimeScope();
        builder.RegisterType<GreetingService>()
            .As<IGreetingService>()
            .InstancePerLifetimeScope();

        using var container = builder.Build();

        var outScope = container.Resolve<IGreetingService>();
        outScope?.Greet();

        using (var scope = container.BeginLifetimeScope())
        {
            var inScope = scope.Resolve<IGreetingService>();
            inScope?.Greet();

            inScope!.IsNotSameReferenceAs(outScope);

            var anotherInScope = scope.Resolve<IGreetingService>();
            anotherInScope?.Greet();

            anotherInScope!.IsSameReferenceAs(inScope);
            anotherInScope!.IsNotSameReferenceAs(outScope);
        }

        mock.Verify(x => x.Print("Hello Ioc world."), Times.Exactly(3));
        mock.VerifyNoOtherCalls();
    }

    [Fact]
    public void WhenRegisteredWithSingleInstance_AlwaysReturnsSameInstance()
    {
        var mock = new Mock<IMessagePrinter>();
        mock.Setup(x => x.Print("Hello Ioc world."));

        var builder = new ContainerBuilder();

        builder.RegisterInstance(mock.Object)
            .As<IMessagePrinter>()
            .ExternallyOwned();
        builder.RegisterType<HelloMessageGenerator>()
            .As<IMessageGenerator>()
            .SingleInstance();
        builder.RegisterType<GreetingService>()
            .As<IGreetingService>()
            .SingleInstance();

        using var container = builder.Build();

        var instance = container.Resolve<IGreetingService>();
        instance?.Greet();

        var another = container.Resolve<IGreetingService>();
        another?.Greet();

        another!.IsSameReferenceAs(instance);

        using (var scope = container.BeginLifetimeScope())
        {
            var inScope = scope.Resolve<IGreetingService>();
            inScope!.Greet();

            inScope!.IsSameReferenceAs(instance);
        }

        mock.Verify(x => x.Print("Hello Ioc world."), Times.Exactly(3));
        mock.VerifyNoOtherCalls();
    }
}
