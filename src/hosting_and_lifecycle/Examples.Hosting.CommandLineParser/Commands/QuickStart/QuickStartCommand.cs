using CommandLine;
using CommandLine.Text;

namespace Examples.Hosting.CommandLineParser.Commands.QuickStart;

/// <summary>
/// Defines the options for the "quick" command, which demonstrates quick start examples with C#.
/// </summary>
[Verb("quick", HelpText = "Quick Start Examples with C#.")]
public class QuickStartCommand : ICommand
{
    [Option('r', "read", Required = true, HelpText = "Input files to be processed.")]
    public IEnumerable<string> InputFiles { get; init; } = default!;

    // Omitting long name, defaults to name of property, ie "--verbose"
    [Option(
      Default = false,
      HelpText = "Prints all messages to standard output.")]
    public bool Verbose { get; init; }

    [Option("stdin",
      Default = false,
      HelpText = "Read from stdin")]
    public bool Stdin { get; init; }

    [Value(0, MetaName = "offset", HelpText = "File offset.")]
    public long? Offset { get; init; }

    [Usage]
    public static IEnumerable<Example> Examples
    {
        get
        {
            yield return new Example($"{Environment.NewLine}Process files with verbose output", new QuickStartCommand
            {
                InputFiles = ["file1.txt", "file2.txt"],
                Verbose = true
            });

            yield return new Example($"{Environment.NewLine}Read from stdin with an offset", new QuickStartCommand
            {
                Stdin = true,
                Offset = 1024
            });
        }
    }
}
