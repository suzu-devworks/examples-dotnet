using System.CommandLine;

namespace Examples.Hosting.CommandLine.Commands.Syntax;

/// <summary>
/// Defines the "syntax" command and its structure.
/// </summary>
public class SyntaxCommand : Command
{
    // Normal Option.
    public readonly Option<int> DelayOption = new("--delay", "-d")
    {
        Description = "An option whose argument is parsed as an int",
        DefaultValueFactory = parseResult => 42,
    };

    public readonly Option<string> MessageOption = new("--message", "-m")
    {
        Description = "An option whose argument is parsed as a string"
    };

    // Global Option.
    public readonly Option<int> GlobalOption = new("--global", "-g")
    {
        Description = "A global option whose argument is parsed as an int",
        Recursive = true,
    };

    // The verbosity option. (with custom validation and parsing logic.)
    public readonly Option<VerbosityLevel> VerbosityOption = new("--verbosity", "-v")
    {
        Description = "Output verbosity level. Allowed values are q[uiet], m[inimal], n[ormal], d[etailed], and diag[nostic].",
        // spell-checker: words uiet inimal ormal etailed nostic
        Recursive = true,
        Arity = ArgumentArity.ZeroOrOne,
        DefaultValueFactory = result =>
        {
            // This runs only when the option isn't specified at all.
            // If the option is specified without a value (for example, `-v`),
            // DefaultValueFactory isn't called and the value is an empty string,
            // which is handled later when mapping to "diagnostic".
            return VerbosityLevel.Normal;
        },
        Validators = { ValidateVerbosity() },
        CustomParser = ConvertVerbosity()
    };

    private static Action<System.CommandLine.Parsing.OptionResult> ValidateVerbosity()
    {
        return result =>
        {
            if (result.Tokens.Count == 0)
            {
                return; // Allow default value.
            }

            var value = result.Tokens.Single().Value.ToLowerInvariant();
            var validValues = new[] { "quiet", "q", "minimal", "m", "normal", "n", "detailed", "d", "diagnostic", "diag" };

            if (!validValues.Contains(value))
            {
                result.AddError($"Argument '{value}' not recognized. Must be one of: 'q[uiet]', 'm[inimal]', 'n[ormal]', 'd[etailed]', 'diag[nostic]'");
            }
        };
    }

    private static Func<System.CommandLine.Parsing.ArgumentResult, VerbosityLevel> ConvertVerbosity()
    {
        return result =>
        {
            // This runs when the option is specified, even if no value is provided.
            // It maps shorthand values to their full form, and defaults an empty value to "diagnostic".
            var value = result.Tokens.Count > 0 ? result.Tokens.Single().Value.ToLowerInvariant() : "diag";
            return value switch
            {
                "quiet" or "q" => VerbosityLevel.Quiet,
                "minimal" or "m" => VerbosityLevel.Minimal,
                "normal" or "n" => VerbosityLevel.Normal,
                "detailed" or "d" => VerbosityLevel.Detailed,
                "diagnostic" or "diag" => VerbosityLevel.Diagnostic,
                _ => VerbosityLevel.Normal
            };
        };
    }

    public enum VerbosityLevel
    {
        Quiet,
        Minimal,
        Normal,
        Detailed,
        Diagnostic
    }

    public readonly Option<bool> QuietOption = new("-q")
    {
        Description = "Set verbosity to quiet (shorthand for --verbosity quiet)",
        Recursive = true
    };

    // Required options
    public readonly Option<FileInfo> FileOutputOption = new("--output")
    {
        Required = true,
        HelpName = "FILEPATH", // To customize the name of an option's argument.
    };

    // Arguments
    public readonly Argument<int> DelayArgument = new("delay")
    {
        Description = "An argument that is parsed as an int.",
        DefaultValueFactory = parseResult => 42
    };

    public readonly Argument<string> MessageArgument = new("message")
    {
        Description = "An argument that is parsed as a string."
    };

    // Arity options
    public readonly Option<string[]> FilesArgument = new("--file")
    {
        Description = "An argument that accepts multiple values (zero or more).",
        Arity = ArgumentArity.ZeroOrMore
    };

    // Boolean options (flags)
    public readonly Option<bool> ForceOption = new("--force", "-f")
    {
        Description = "A boolean option that doesn't take an argument. If specified, its value is true; otherwise, false.",
    };

    // Help customization (Configures the option to accept only the specified values)
    public readonly Option<ConsoleColor> ForegroundColorOption = InitializeForegroundColorOption();
    public static Option<ConsoleColor> InitializeForegroundColorOption()
    {
        var option = new Option<ConsoleColor>("--color")
        {
            Description = "Specifies the foreground color of console output",
            DefaultValueFactory = _ => ConsoleColor.White,
        };

        option.AcceptOnlyFromAmong(
            ConsoleColor.Black.ToString(),
            ConsoleColor.White.ToString(),
            ConsoleColor.Red.ToString(),
            ConsoleColor.Yellow.ToString()
        );

        return option;
    }

    // Cascading options (Options that can be specified multiple times)
    public readonly Option<bool> CascadingVerboseOption = new("-V")
    {
        Description = "Cascading verbose flag. Specify multiple times to increase verbosity.",
        Arity = ArgumentArity.ZeroOrMore,
        CustomParser = result => true,
        Validators = {
            result =>
            {
                if (result.Tokens.Count != 0)
                {
                    result.AddError("The -V option does not accept any arguments. Specify -V multiple times to increase verbosity.");
                }
            }
        },
    };

    public SyntaxCommand() : base("syntax", "Commands, options, and arguments.")
    {
        Options.Add(GlobalOption);
        Options.Add(VerbosityOption);
        Options.Add(QuietOption);

        Aliases.Add("syn");

        // Subcommands for checking, such as global.
        Subcommands.Add(new("sub1", "First-level subcommand")
        {
            new Command("sub1a", "Second level subcommand A")
        });

        Subcommands.Add(new("opts")
        {
            DelayOption,
            MessageOption,
            ForegroundColorOption,
            FileOutputOption,
            FilesArgument,
            ForceOption,
        });
        Subcommands.FirstOrDefault(c => c.Name == "opts")
            ?.SetAction(DisplayOptions);

        Subcommands.Add(new("args")
        {
            DelayArgument,
            MessageArgument,
        });
        Subcommands.FirstOrDefault(c => c.Name == "args")
            ?.SetAction(DisplayArguments);

        Subcommands.Add(new("cascade")
        {
            CascadingVerboseOption
        });
        Subcommands.FirstOrDefault(c => c.Name == "cascade")
            ?.SetAction(parseResult =>
            {
                var value = parseResult.GetValue(CascadingVerboseOption);
                var count = parseResult.Tokens.Count(x => x.Value == "-V");
                System.Console.WriteLine($"Cascading verbose count: {value}, {count}");
                return 0;
            });
    }

    private void DisplayOptions(ParseResult parseResult)
    {
        var delay = parseResult.GetValue(DelayOption);
        var message = parseResult.GetValue(MessageOption);

        var global = parseResult.GetValue(GlobalOption);
        var verbosity = parseResult.GetValue(VerbosityOption);
        var quiet = parseResult.GetValue(QuietOption);

        var outputFile = parseResult.GetValue(FileOutputOption);
        var force = parseResult.GetValue(ForceOption);
        var files = parseResult.GetValue(FilesArgument) ?? [];

        System.Console.WriteLine($"Delay: {delay}");
        System.Console.WriteLine($"Message: {message}");
        System.Console.WriteLine($"Foreground color: {parseResult.GetValue(ForegroundColorOption)}");

        System.Console.WriteLine($"Global: {global}");
        System.Console.WriteLine($"Verbosity: {verbosity}");
        System.Console.WriteLine($"Quiet: {quiet}");

        System.Console.WriteLine($"Output file: {outputFile}");
        System.Console.WriteLine($"Force: {force}");
        System.Console.WriteLine($"Files: {string.Join(", ", files)}");
    }

    private void DisplayArguments(ParseResult parseResult)
    {
        var delay = parseResult.GetValue(DelayArgument);
        var message = parseResult.GetValue(MessageArgument);

        System.Console.WriteLine($"Delay: {delay}");
        System.Console.WriteLine($"Message: {message}");
    }
}
