using Autofac;
using Examples.Tests.Autofac.Fixtures.Greetings;

namespace Examples.Tests.Autofac.BasicUsage;

public class MultipleRegisteredServicesTests
{
    [Fact]
    public void WhenMultipleEntriesRegisteredWithSameInterface_ReturnsAsIEnumerable()
    {
        var mock = new Mock<IMessagePrinter>();
        mock.Setup(x => x.Print("Hello Autofac world 1st."));
        mock.Setup(x => x.Print("Hello Autofac world 2nd."));
        mock.Setup(x => x.Print("Hello Autofac world 3rd."));

        var builder = new ContainerBuilder();

        builder.RegisterInstance(mock.Object)
            .As<IMessagePrinter>()
            .ExternallyOwned();

        builder.RegisterTypes(
                typeof(MyMessageGenerator1),
                typeof(MyMessageGenerator2),
                typeof(MyMessageGenerator3))
            .As<IMessageGenerator>()
            .SingleInstance();

        builder
            .RegisterType<GreetingService>()
            .As<IGreetingService>()
            .SingleInstance();

        using var container = builder.Build();

        var instance = container.Resolve<IGreetingService>();
        instance?.Greet();

        mock.Verify(x => x.Print("Hello Autofac world 1st."), Times.Exactly(1));
        mock.Verify(x => x.Print("Hello Autofac world 2nd."), Times.Exactly(1));
        mock.Verify(x => x.Print("Hello Autofac world 3rd."), Times.Exactly(1));
        mock.VerifyNoOtherCalls();
    }


    private class MyMessageGenerator1 : IMessageGenerator
    {
        public string Generate() => "Hello Autofac world 1st.";
    }

    private class MyMessageGenerator2 : IMessageGenerator
    {
        public string Generate() => "Hello Autofac world 2nd.";
    }

    private class MyMessageGenerator3 : IMessageGenerator
    {
        public string Generate() => "Hello Autofac world 3rd.";
    }
}
