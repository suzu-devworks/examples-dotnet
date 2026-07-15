using System.CommandLine;

namespace Examples.Hosting.CommandLine.Commands.Quotes;

/// <summary>
/// Defines the "quotes" command and its structure.
/// </summary>
public class QuotesCommand : Command
{
    public readonly Option<FileInfo> FileOption = new("--file")
    {
        Description = "An option whose argument is parsed as a FileInfo",
        DefaultValueFactory = _ => new FileInfo("sampleQuotes.txt"),
        CustomParser = result =>
        {
            var file = new FileInfo(result.Tokens.Single().Value);
            if (!file.Exists)
            {
                result.AddError("File does not exist");
            }
            return file;
        },
        Recursive = true
    };

    public QuotesCommand() : base("quotes", "Work with a file that contains quotes.")
    {
        Options.Add(FileOption);
    }
}
