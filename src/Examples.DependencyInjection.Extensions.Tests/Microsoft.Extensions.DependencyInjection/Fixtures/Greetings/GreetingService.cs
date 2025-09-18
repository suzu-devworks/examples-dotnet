namespace Examples.Tests.Microsoft.Extensions.DependencyInjection.Fixtures.Greetings;

public sealed class GreetingService(
    IMessagePrinter printer,
    IEnumerable<IMessageGenerator> generators) : IGreetingService
{
    private readonly IMessagePrinter _printer = printer;
    private readonly IEnumerable<IMessageGenerator> _generators = generators;

    public void Greet()
    {
        foreach (var greater in _generators)
        {
            _printer.Print(greater.Generate());
        }
    }
}
