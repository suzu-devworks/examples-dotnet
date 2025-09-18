namespace Examples.Tests.System.Composition.Fixtures.MyServices;

public class MyService(
    IMessagePrinter messagePrinter,
    IEnumerable<IMessageGenerator> messageGenerators) : IMyService
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
