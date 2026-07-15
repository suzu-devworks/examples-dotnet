using System.CommandLine;
using Examples.Hosting.CommandLine.Commands.Quotes;
using Examples.Hosting.CommandLine.Commands.Syntax;
using Examples.Hosting.CommandLine.Handlers;
using Examples.Hosting.CommandLine.Handlers.Quotes;
using Examples.Hosting.CommandLine.Services.Quotes;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

var builder = Host.CreateApplicationBuilder(args);

builder.Logging.ClearProviders();
builder.Logging.AddConsole();

builder.Services.AddScoped<IQuotesService, QuotesService>();
builder.Services.AddScoped<QuotesCommandHandler>();

using var host = builder.Build();

RootCommand rootCommand = new RootCommand("Sample app for System.CommandLine")
{
    new QuotesCommand
    {
        new AddCommand(),
        new DeleteCommand(),
        new ReadCommand(),
    },
    new SyntaxCommand(),
};

host.MapCommandHandlers(rootCommand, builder =>
{
    builder.AddHandler<AddCommand, QuotesCommandHandler>();
    builder.AddHandler<DeleteCommand, QuotesCommandHandler>();
    builder.AddHandler<ReadCommand, QuotesCommandHandler>();
});

return await rootCommand.Parse(args).InvokeAsync();
