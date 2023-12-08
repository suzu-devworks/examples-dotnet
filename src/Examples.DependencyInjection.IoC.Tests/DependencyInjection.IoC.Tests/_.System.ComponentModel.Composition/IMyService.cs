namespace Examples.DependencyInjection.IoC.Tests._.System.ComponentModel.Composition;

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
