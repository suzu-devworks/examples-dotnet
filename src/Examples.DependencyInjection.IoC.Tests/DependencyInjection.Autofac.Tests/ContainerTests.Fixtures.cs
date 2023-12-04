using Examples.DependencyInjection.IoC.Tests;

namespace Examples.DependencyInjection.Autofac.Tests;

public partial class ContainerTests
{

    private class MyMessageGenerator1 : IMessageGenerator
    {
        public string Generate() => "Hello Autofac Ioc world 1st.";
    }

    private class MyMessageGenerator2 : IMessageGenerator
    {
        public string Generate() => "Hello Autofac Ioc world 2nd.";
    }

    private class MyMessageGenerator3 : IMessageGenerator
    {
        public string Generate() => "Hello Autofac Ioc world 3rd.";
    }

    private class MyService : IMyService
    {
        private readonly IMessagePrinter _messagePrinter;
        private readonly IEnumerable<IMessageGenerator> _messageGenerators;

        public MyService(
            IMessagePrinter messagePrinter,
            IEnumerable<IMessageGenerator> messageGenerators)
        {
            _messagePrinter = messagePrinter;
            _messageGenerators = messageGenerators;
        }

        public void Greet()
        {
            foreach (var greater in _messageGenerators)
            {
                _messagePrinter.Print(greater.Generate());
            }
        }
    }

}
