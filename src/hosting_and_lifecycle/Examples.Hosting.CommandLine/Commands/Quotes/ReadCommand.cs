using System.CommandLine;

namespace Examples.Hosting.CommandLine.Commands.Quotes;

/// <summary>
/// Defines the "read" subcommand of the "quotes" command.
/// </summary>
public class ReadCommand : Command
{
    public readonly Option<int> DelayOption = new("--delay")
    {
        Description = "Delay between lines, specified as milliseconds per character in a line",
        DefaultValueFactory = parseResult => 42
    };

    public readonly Option<ConsoleColor> FgColorOption = new("--fg-color")
    {
        Description = "Foreground color of text displayed on the console",
        DefaultValueFactory = parseResult => ConsoleColor.White
    };

    public readonly Option<bool> LightModeOption = new("--light-mode")
    {
        Description = "Background color of text displayed on the console: default is black, light mode is white"
    };

    public ReadCommand() : base("read", "Read and display the file.")
    {
        Options.Add(DelayOption);
        Options.Add(FgColorOption);
        Options.Add(LightModeOption);
    }
}
