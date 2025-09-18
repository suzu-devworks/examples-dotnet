namespace Examples.Tests.System.Composition.Fixtures.MyServices;

public interface IMessagePrinter
{
    void Print(string message);
}

public interface IMessageGenerator
{
    string Generate();
}

public interface IMyService
{
    void Greet();
}
