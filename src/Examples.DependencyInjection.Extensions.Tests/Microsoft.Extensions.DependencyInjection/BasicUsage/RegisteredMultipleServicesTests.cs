using Examples.Tests.Microsoft.Extensions.DependencyInjection.Fixtures.Greetings;
using Microsoft.Extensions.DependencyInjection;

namespace Examples.Tests.Microsoft.Extensions.DependencyInjection.BasicUsage;

public class MultipleRegisteredServicesTests
{
    [Fact]
    public void WhenMultipleServicesRegisteredWithSameInterface_ReturnsAsIEnumerable()
    {
        var mock = new Mock<IMessagePrinter>();
        mock.Setup(x => x.Print("Hello DI world 1st."));
        mock.Setup(x => x.Print("Hello DI world 2nd."));
        mock.Setup(x => x.Print("Hello DI world 3rd."));

        var services = new ServiceCollection();
        services.AddSingleton<IMessagePrinter>(_ => mock.Object);
        services.AddSingleton<IMessageGenerator, MyMessageGenerator1>();
        services.AddSingleton<IMessageGenerator, MyMessageGenerator2>();
        services.AddSingleton<IMessageGenerator, MyMessageGenerator3>();
        services.AddTransient<IGreetingService, GreetingService>();

        using var provider = services.BuildServiceProvider();

        var service = provider.GetService<IGreetingService>();
        service?.Greet();

        mock.Verify(x => x.Print("Hello DI world 1st."), Times.Exactly(1));
        mock.Verify(x => x.Print("Hello DI world 2nd."), Times.Exactly(1));
        mock.Verify(x => x.Print("Hello DI world 3rd."), Times.Exactly(1));
        mock.VerifyNoOtherCalls();
    }



    private class MyMessageGenerator1 : IMessageGenerator
    {
        public string Generate() => "Hello DI world 1st.";
    }

    private class MyMessageGenerator2 : IMessageGenerator
    {
        public string Generate() => "Hello DI world 2nd.";
    }

    private class MyMessageGenerator3 : IMessageGenerator
    {
        public string Generate() => "Hello DI world 3rd.";
    }

}
