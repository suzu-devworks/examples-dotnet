using System.Text;
using CommandLine;

namespace Examples.Hosting.CommandLineParser.Commands.Syntax;

/// <summary>
/// Defines the options for the "syntax" command.
/// </summary>
[Verb("syntax", HelpText = "Parsed Options and Value.")]
public class SyntaxCommand : ICommand
{
    // [Value] Attribute

    [Value(0, HelpText = "An integer value.")]
    public int IntValue { get; init; }

    // Omitting the minimum and maximum constraints specifies all available values,
    // so it should be used at the end of the argument.
    [Value(1, Min = 1, Max = 3, HelpText = "A list of string values (1 to 3).")]
    public IEnumerable<string> StringValues { get; init; } = default!;

    [Value(2, Default = 3.14, HelpText = "A double value with a default of 3.14.")]
    public double DoubleValue { get; init; }

    [Option(HelpText = "The long name will be inferred from the member's name.")]
    public string UserId { get; set; } = default!;

    [Option('t', Separator = ':', HelpText = "A list of string values separated by ':'.")]
    public IEnumerable<string> Types { get; set; } = default!;

    public override string ToString()
    {
        var builder = new StringBuilder("Parsed Options and Value:");
        builder.AppendLine();

        builder.AppendLine($"  IntValue: {IntValue}");
        builder.AppendLine($"  StringValues: {string.Join(", ", StringValues)}");
        builder.AppendLine($"  DoubleValue: {DoubleValue:F2}");
        builder.AppendLine($"  UserId: {UserId}");
        builder.AppendLine($"  Types: {string.Join(", ", Types)}");

        return builder.ToString();
    }
}
