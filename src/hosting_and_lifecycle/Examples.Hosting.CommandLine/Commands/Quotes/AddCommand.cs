using System.CommandLine;

namespace Examples.Hosting.CommandLine.Commands.Quotes;

/// <summary>
/// Defines the "add" and "insert" subcommand of the "quotes" command.
/// </summary>
public class AddCommand : Command
{
    public readonly Argument<string> QuoteArgument = new("quote")
    {
        Description = "Text of quote."
    };

    public readonly Argument<string> BylineArgument = new("byline")
    {
        Description = "Byline of quote."
    };

    public AddCommand() : base("add", "Add an entry to the file.")
    {
        Arguments.Add(QuoteArgument);
        Arguments.Add(BylineArgument);

        Aliases.Add("insert");
    }
}
