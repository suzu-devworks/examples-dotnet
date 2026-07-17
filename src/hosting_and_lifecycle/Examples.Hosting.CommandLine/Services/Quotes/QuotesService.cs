namespace Examples.Hosting.CommandLine.Services.Quotes;

/// <summary>
/// Implements the IQuotesService interface to provide functionality for managing quotes.
/// </summary>
public class QuotesService : IQuotesService
{
    /// <inheritdoc/>
    public void ReadFile(FileInfo file, int delay, ConsoleColor fgColor, bool lightMode)
    {
        System.Console.BackgroundColor = lightMode ? ConsoleColor.White : ConsoleColor.Black;
        System.Console.ForegroundColor = fgColor;
        foreach (string line in File.ReadLines(file.FullName))
        {
            System.Console.WriteLine(line);
            Thread.Sleep(TimeSpan.FromMilliseconds(delay * line.Length));
        }
    }

    /// <inheritdoc/>
    public void DeleteFromFile(FileInfo file, string[] searchTerms)
    {
        System.Console.WriteLine("Deleting from file");

        var lines = File.ReadLines(file.FullName)
            .Where(line => searchTerms.All(s => !line.Contains(s)))
            .ToArray();
        File.WriteAllLines(file.FullName, lines);
    }

    /// <inheritdoc/>
    public void AddToFile(FileInfo file, string quote, string byline)
    {
        System.Console.WriteLine("Adding to file");

        using StreamWriter writer = file.AppendText();
        writer.WriteLine($"{Environment.NewLine}{Environment.NewLine}{quote}");
        writer.WriteLine($"{Environment.NewLine}-{byline}");
    }

}
