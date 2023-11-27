using System.CommandLine;

namespace Examples.Hosting.Console;

public static class CommandLineExtensions
{
    public static RootCommand AddHandler(this RootCommand root, Func<Task> action)
    {
        root.SetHandler(action);
        return root;
    }

    public static RootCommand AddCommand(this RootCommand root, Command sub, Func<Task> action)
    {
        sub.SetHandler(action);
        root.AddCommand(sub);
        return root;
    }

}
