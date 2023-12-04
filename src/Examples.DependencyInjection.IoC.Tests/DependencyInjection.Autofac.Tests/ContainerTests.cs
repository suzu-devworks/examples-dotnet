using Autofac;
using Examples.DependencyInjection.IoC.Tests;

namespace Examples.DependencyInjection.Autofac.Tests;

public partial class ContainerTests
{
    [Fact]
    public void WhenMultipleInjection()
    {
        var mock = new Mock<IMessagePrinter>();
        mock.Setup(x => x.Print("Hello Autofac Ioc world 1st."));
        mock.Setup(x => x.Print("Hello Autofac Ioc world 2nd."));
        mock.Setup(x => x.Print("Hello Autofac Ioc world 3rd."));

        // Create your builder.
        var builder = new ContainerBuilder();

        builder
            .RegisterInstance(mock.Object)
            .As<IMessagePrinter>()
              .ExternallyOwned();

        builder
            .RegisterTypes(typeof(MyMessageGenerator1), typeof(MyMessageGenerator2), typeof(MyMessageGenerator3))
            .As<IMessageGenerator>()
            .SingleInstance();

        builder
            .RegisterType<MyService>()
            .As<IMyService>();

        using var container = builder.Build();

        // Use.
        var service = container.Resolve<IMyService>();
        service?.Greet();

        var other = container.Resolve<IMyService>();
        other?.Greet();

        // The instance is also different.
        object.ReferenceEquals(service, other).IsFalse();

        mock.Verify(x => x.Print("Hello Autofac Ioc world 1st."), Times.Exactly(2));
        mock.Verify(x => x.Print("Hello Autofac Ioc world 2nd."), Times.Exactly(2));
        mock.Verify(x => x.Print("Hello Autofac Ioc world 3rd."), Times.Exactly(2));
        mock.VerifyNoOtherCalls();
        return;
    }

}
