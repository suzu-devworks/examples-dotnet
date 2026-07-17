using Examples.Metaprogramming.Helpers;

namespace Examples.Metaprogramming.ReflectionEmit.Tests.Learns;

[Collection(TestCollectionNames.UseSystemConsole)]
public class HelloWorldBuilderTests
{
    [Fact]
    public void WhenCallingGeneratedHelloWorldProgram_OutToConsoleAsExpected()
    {
        using var console = new ConsoleRedirection();

        Type programClass = new HelloWorldBuilder().Build();
        programClass.GetMethod("Main")!.Invoke(null, null);

        Assert.Contains("Hello Reflection.Emit World", console.GetOutput());
    }
}
