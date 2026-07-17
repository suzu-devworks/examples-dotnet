using CommandLine;
using Examples.Hosting.CommandLineParser.Commands;
using Examples.Hosting.CommandLineParser.Commands.QuickStart;
using Examples.Hosting.CommandLineParser.Commands.Syntax;
using Examples.Hosting.CommandLineParser.Handlers;
using Examples.Hosting.CommandLineParser.Handlers.QuickStart;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

static async Task<int> StartCommandWithHost<TCommand, THandler>(string[] args, TCommand command,
    Action<IServiceCollection>? configureCommandServices = null)
         where TCommand : class, ICommand
         where THandler : class, ICommandHandler
{
    var builder = Host.CreateApplicationBuilder(args);

    builder.Logging.ClearProviders();
    builder.Logging.AddConsole();

    // Register command-specific services
    configureCommandServices?.Invoke(builder.Services);

    builder.Services.AddSingleton(command);
    builder.Services.AddScoped<THandler>();

    using var host = builder.Build();

    CancellationTokenSource cancellationTokenSource = new();
    Console.CancelKeyPress += (sender, eventArgs) =>
    {
        Console.Error.WriteLine("Cancellation requested. Shutting down...");
        eventArgs.Cancel = true; // Prevent the process from terminating immediately
        cancellationTokenSource.Cancel();
    };

    using var scope = host.Services.CreateScope();
    var worker = scope.ServiceProvider.GetRequiredService<THandler>();
    await worker.ExecuteAsync(cancellationToken: cancellationTokenSource.Token);
    return 0;
}

static async Task<int> Display(ICommand command)
{
    // Fallback for commands that do not have a specific handler, simply display the command properties
    Console.WriteLine(command);
    return await Task.FromResult(0);
}

var parserResult = Parser.Default.ParseArguments<QuickStartCommand, SyntaxCommand, SyntaxOptionsCommand,
    SyntaxExclusiveOptionCommand, SyntaxGroupingOptionCommand>(args);

return await parserResult.MapResult(
    (QuickStartCommand command) => StartCommandWithHost<QuickStartCommand, QuickStartCommandHandler>(args, command),
    (ICommand command) => Display(command),
    notParsedFunc: errors => Task.FromResult(1)
    );
