using System.ComponentModel.Composition;

namespace Examples.DependencyInjection.IoC.Tests._.System.ComponentModel.Composition;

public partial class CompositionContainerTests
{

    [Export(typeof(IMessageGenerator))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    private class MyMessageGenerator1 : IMessageGenerator
    {
        public string Generate() => "Hello MEF DI world 1st.";
    }

    [Export(typeof(IMessageGenerator))]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    private class MyMessageGenerator2 : IMessageGenerator
    {
        public string Generate() => "Hello MEF DI world 2nd.";
    }

    [Export(typeof(IMessageGenerator))]
    [PartCreationPolicy(CreationPolicy.Any)]
    private class MyMessageGenerator3 : IMessageGenerator
    {
        public string Generate() => "Hello MEF DI world 3rd.";
    }

    [Export(typeof(IMyService))]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    private class MyService : IMyService
    {
        private readonly IMessagePrinter _messagePrinter;
        private readonly IEnumerable<IMessageGenerator> _messageGenerators;

        [ImportingConstructor]
        public MyService(
            [Import] IMessagePrinter messagePrinter,
            [ImportMany] IEnumerable<IMessageGenerator> messageGenerators)
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
