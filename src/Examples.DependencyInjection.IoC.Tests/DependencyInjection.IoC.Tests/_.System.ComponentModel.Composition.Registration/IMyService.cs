namespace Examples.DependencyInjection.IoC.Tests._.System.ComponentModel.Composition.Registration;

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
