using Examples.Hosting.Console.Startup;

using var cts = new CancellationTokenSource();
Console.CancelKeyPress += (s, e) =>
{
    Console.WriteLine("Cancellation request received. Starting graceful shutdown...");
    e.Cancel = true;
    cts.Cancel();
};

if (args.Contains("--host-app-hosted", StringComparer.OrdinalIgnoreCase))
{
    return await HostApplicationBuilderStartup.RunWithHostedAsync(args);
}

if (args.Contains("--host-app", StringComparer.OrdinalIgnoreCase))
{
    return await HostApplicationBuilderStartup.RunAsync(args, cts.Token);
}

if (args.Contains("--host", StringComparer.OrdinalIgnoreCase))
{
    return await HostBuilderStartup.RunAsync(args, cts.Token);
}

if (args.Contains("--without", StringComparer.OrdinalIgnoreCase))
{
    return await WithoutHostingStartup.RunAsync(args, cts.Token);
}

Console.ForegroundColor = ConsoleColor.Red;
Console.Error.WriteLine("No valid startup option provided. Use --host, --host-app, --host-app-hosted, or --without.");
Console.ResetColor();
Console.Error.WriteLine();
Console.Error.WriteLine("Example: dotnet run -- --host-app");

return 1;
