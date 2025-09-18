namespace Examples.Tests.Autofac.Fixtures.Greetings;

public class HelloMessageGenerator : IMessageGenerator
{
    public string Generate() => "Hello Ioc world.";
}
