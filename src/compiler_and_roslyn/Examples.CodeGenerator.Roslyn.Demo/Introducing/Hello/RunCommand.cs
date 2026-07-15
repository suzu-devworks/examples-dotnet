using System.CommandLine;

namespace Examples.CodeGenerator.Roslyn.Introducing.Hello;

public class RunCommand : Command
{
    public RunCommand() : base("hello", "Runs the HelloWorldGenerator sample.")
    {
        SetAction((context) =>
        {
            HelloWorldGenerated.HelloWorld.SayHello(Console.Out);
        });
    }
}
