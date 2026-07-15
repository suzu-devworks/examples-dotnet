#!/usr/bin/env dotnet run
#:package ConsoleAppFramework@5.*

// Chap07-02.cs
// [CancellationToken(Gracefully Shutdown) and Timeout](https://github.com/Cysharp/ConsoleAppFramework#cancellationtokengracefully-shutdown-and-timeout)
//
// If a CancellationToken is present, it internally uses PosixSignalRegistration
// to hook SIGINT/SIGTERM and sets the CancellationToken to a canceled state.
// Additionally, it prevents forced termination to allow for a graceful shutdown.

using ConsoleAppFramework;

// // Internal code that is generated:
// using var posixSignalHandler = PosixSignalHandler.Register(ConsoleApp.Timeout);
// var arg0 = posixSignalHandler.Token;

await ConsoleApp.RunAsync(args, async (CancellationToken cancellationToken) =>
{
    try
    {
        for (int i = 0; i < 10; i++)
        {
            Console.WriteLine($"Running main task iteration {i + 1}/10.");
            await Task.Delay(TimeSpan.FromSeconds(1), cancellationToken);
        }
    }
    catch (OperationCanceledException)
    {
        Console.WriteLine("Main task was cancelled.");
    }
});
