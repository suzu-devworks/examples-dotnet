using System.CommandLine;

namespace Examples.Hosting.Console;

public static class CommandLineExtensions
{
    public static RootCommand AddCommand(this RootCommand root, Command sub, Func<ParseResult, CancellationToken, Task> action)
    {
        sub.SetAction(action);
        root.Subcommands.Add(sub);
        return root;
    }

}
