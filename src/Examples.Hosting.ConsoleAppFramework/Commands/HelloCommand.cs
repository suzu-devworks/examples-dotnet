using ConsoleAppFramework;
using Examples.Hosting.ConsoleAppFramework.Services.Hello;
using Microsoft.Extensions.Logging;

namespace Examples.Hosting.ConsoleAppFramework.Commands;

[RegisterCommands]
public class HelloCommand(
    IHelloService helloService,
    ILogger<HelloCommand> logger
)
{
    public void SayHello(string name)
    {
        helloService.SayHello(name);
        logger.LogInformation("Said hello to {Name}", name);
    }
}
