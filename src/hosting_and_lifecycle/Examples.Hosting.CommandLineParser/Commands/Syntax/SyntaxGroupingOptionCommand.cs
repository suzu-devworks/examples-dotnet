using System.Text;
using CommandLine;

namespace Examples.Hosting.CommandLineParser.Commands.Syntax;

/// <summary>
/// Defines the options for the "syntax-grouping" command.
/// </summary>
[Verb("syntax-grouping", HelpText = "Grouping Options Syntax Example.")]
public class SyntaxGroupingOptionCommand : ICommand
{
    // An option group represents a group of options which are optional,
    // but at least one should be available.

    [Option(Group = "append", HelpText = "Prefix to append to file name")]
    public string Prefix { get; init; } = default!;

    [Option(Group = "append", HelpText = "Suffix to append to file name")]
    public string Suffix { get; init; } = default!;

    [Option("source", HelpText = "The path of a file to rename")]
    public string FilePath { get; init; } = default!;

    public override string ToString()
    {
        var builder = new StringBuilder("Grouping Options Syntax Example:");
        builder.AppendLine();

        builder.AppendLine($"  Prefix: {Prefix}");
        builder.AppendLine($"  Suffix: {Suffix}");
        builder.AppendLine($"  FilePath: {FilePath}");

        return builder.ToString();
    }
}
