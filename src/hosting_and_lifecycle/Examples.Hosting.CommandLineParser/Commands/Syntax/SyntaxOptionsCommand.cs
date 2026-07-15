using System.Text;
using CommandLine;

namespace Examples.Hosting.CommandLineParser.Commands.Syntax;

/// <summary>
/// Defines the options for the "syntax-options" command.
/// </summary>
[Verb("syntax-options", HelpText = "Options Syntax Example.")]
public class SyntaxOptionsCommand : ICommand
{
    // Named Options

    [Option(HelpText = "Switch option.")]
    public bool CheckIn { get; init; }

    [Option(Default = (bool)true, HelpText = "Switch option with nullable bool type.")]
    public bool? Visible { get; init; }

    [Option("device-name", HelpText = "Scalar option.")]
    public string DeviceName { get; init; } = default!;

    [Option(HelpText = "Sequence option.")]
    public IEnumerable<string> Files { get; init; } = default!;

    // Enum Flags option

    [Flags]
    public enum TestFlagEnum
    {
        None = 0x0,
        ValueA = 0x1,
        ValueB = 0x2,
        ValueC = 0x4,
        ValueD = 0x8,
    }

    // To make an Enum case-insensitive,
    // configure the Parser with `CaseInsensitiveEnumValues = true`.

    [Option(HelpText = "Enum Flags option.")]
    public TestFlagEnum Flags { get; init; }

    public override string ToString()
    {
        var builder = new StringBuilder("Options Syntax Example:");
        builder.AppendLine();

        builder.AppendLine($"  CheckIn: {CheckIn}");
        builder.AppendLine($"  Visible: {Visible}");
        builder.AppendLine($"  DeviceName: {DeviceName}");
        builder.AppendLine($"  Files: {string.Join(", ", Files)}");
        builder.AppendLine($"  Flags: {Flags}");

        return builder.ToString();
    }
}
