using System.ComponentModel.Composition;

namespace Examples.Tests.System.ComponentModel.Composition.Fixtures.MyServices;

[Export(typeof(IMyService))]
[PartCreationPolicy(CreationPolicy.NonShared)]
[method: ImportingConstructor]
public class MyService(
    [Import] IMessagePrinter messagePrinter,
    [ImportMany] IEnumerable<IMessageGenerator> messageGenerators) : IMyService
{
    private readonly IMessagePrinter _messagePrinter = messagePrinter;
    private readonly IEnumerable<IMessageGenerator> _messageGenerators = messageGenerators;

    public void Greet()
    {
        foreach (var greater in _messageGenerators)
        {
            _messagePrinter.Print(greater.Generate());
        }
    }
}
