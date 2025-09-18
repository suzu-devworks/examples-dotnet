namespace Examples.Tests.System.ComponentModel.Composition.Registration.Fixtures.MyServices;

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
