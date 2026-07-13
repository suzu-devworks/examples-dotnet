namespace Examples.Hosting.CommandLine.Services.Quotes;

/// <summary>
/// Defines a contract for a service that manages quotes.
/// </summary>
public interface IQuotesService
{
    /// <summary>
    /// Adds a quote to the specified file, with the given quote text and byline.
    /// </summary>
    /// <param name="file">The file to which the quote will be added.</param>
    /// <param name="quote">The text of the quote to add.</param>
    /// <param name="byline">The byline or author of the quote.</param>
    void AddToFile(FileInfo file, string quote, string byline);

    /// <summary>
    /// Deletes lines from the specified file that contain any of the provided search terms.
    /// </summary>
    /// <param name="file">The file from which lines will be deleted.</param>
    /// <param name="searchTerms">An array of search terms; lines containing any of these will be deleted.</param>
    void DeleteFromFile(FileInfo file, string[] searchTerms);

    /// <summary>
    /// Reads the specified file and writes its contents to the console.
    /// </summary>
    /// <param name="file">The file to read and display.</param>
    /// <param name="delay">The delay in milliseconds between displaying each line.</param>
    /// <param name="fgColor">The foreground color to use when displaying text.</param>
    /// <param name="lightMode">Whether to use light mode for display formatting.</param>
    void ReadFile(FileInfo file, int delay, ConsoleColor fgColor, bool lightMode);
}
