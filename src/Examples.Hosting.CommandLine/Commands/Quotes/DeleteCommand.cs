using System.CommandLine;

namespace Examples.Hosting.CommandLine.Commands.Quotes;

/// <summary>
/// Defines the "delete" subcommand of the "quotes" command.
/// </summary>
public class DeleteCommand : Command
{
    public readonly Option<string[]> SearchTermsOption = new("--search-terms")
    {
        Description = "Strings to search for when deleting entries",
        Required = true,
        AllowMultipleArgumentsPerToken = true
    };

    public DeleteCommand() : base("delete", "Delete lines from the file.")
    {
        Options.Add(SearchTermsOption);
    }
}
