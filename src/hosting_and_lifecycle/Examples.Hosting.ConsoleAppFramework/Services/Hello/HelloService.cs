namespace Examples.Hosting.ConsoleAppFramework.Services.Hello;

public class HelloService : IHelloService
{
    private readonly TextWriter _output = System.Console.Out;
    public void SayHello(string name)
    {
        _output.WriteLine($"Hello, {name}!");
    }
}
