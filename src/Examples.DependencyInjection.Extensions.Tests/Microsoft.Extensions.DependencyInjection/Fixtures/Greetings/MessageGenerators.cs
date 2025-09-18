namespace Examples.Tests.Microsoft.Extensions.DependencyInjection.Fixtures.Greetings;

public class HelloMessageGenerator : IMessageGenerator
{
    public string Generate() => "Hello Ioc world.";
}
