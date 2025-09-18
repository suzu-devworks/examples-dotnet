namespace Examples.Tests.Microsoft.Extensions.DependencyInjection.Fixtures.Greetings;

public interface IMessagePrinter
{
    void Print(string message);
}

public interface IMessageGenerator
{
    string Generate();
}

public interface IGreetingService
{
    void Greet();
}
