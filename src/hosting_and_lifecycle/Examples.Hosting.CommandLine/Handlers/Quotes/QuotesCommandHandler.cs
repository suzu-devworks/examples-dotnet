using System.CommandLine;
using Examples.Hosting.CommandLine.Commands.Quotes;
using Examples.Hosting.CommandLine.Services.Quotes;

namespace Examples.Hosting.CommandLine.Handlers.Quotes;

/// <summary>
/// Implements command handlers for the "quotes" command and its subcommands.
/// </summary>
/// <param name="quotesService"></param>
public class QuotesCommandHandler(IQuotesService quotesService)
    : ICommandHandler<AddCommand>, ICommandHandler<DeleteCommand>, ICommandHandler<ReadCommand>
{
    /// <inheritdoc/>
    public async Task<int> InvokeAsync(AddCommand command, ParseResult parseResult, CancellationToken ct)
    {
        var file = parseResult.GetValue<FileInfo>("--file")!;
        var quote = parseResult.GetValue(command.QuoteArgument) ?? "";
        var byline = parseResult.GetValue(command.BylineArgument) ?? "";

        quotesService.AddToFile(file, quote, byline);

        return await Task.FromResult(0);
    }

    /// <inheritdoc/>
    public async Task<int> InvokeAsync(DeleteCommand command, ParseResult parseResult, CancellationToken ct)
    {
        var file = parseResult.GetValue<FileInfo>("--file")!;
        var searchTerms = parseResult.GetValue(command.SearchTermsOption) ?? [];

        quotesService.DeleteFromFile(file, searchTerms);

        return await Task.FromResult(0);
    }

    /// <inheritdoc/>
    public async Task<int> InvokeAsync(ReadCommand command, ParseResult parseResult, CancellationToken ct)
    {
        var file = parseResult.GetValue<FileInfo>("--file")!;
        var delay = parseResult.GetValue(command.DelayOption);
        var fgColor = parseResult.GetValue(command.FgColorOption);
        var lightMode = parseResult.GetValue(command.LightModeOption);

        quotesService.ReadFile(file, delay, fgColor, lightMode);

        return await Task.FromResult(0);
    }
}
