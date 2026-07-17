#!/usr/bin/env dotnet run
#:package ConsoleAppFramework@5.*

// Chap07-01.cs
// [CancellationToken(Gracefully Shutdown) and Timeout](https://github.com/Cysharp/ConsoleAppFramework#cancellationtokengracefully-shutdown-and-timeout)
//
// If a CancellationToken is not passed, the application is immediately forced to terminate
// when an interruption command (Ctrl+C) is received.

using ConsoleAppFramework;

await ConsoleApp.RunAsync(args, async () =>
{
    try
    {
        for (int i = 0; i < 10; i++)
        {
            Console.WriteLine($"Running main task iteration {i + 1}/10.");
            await Task.Delay(TimeSpan.FromSeconds(1));
        }
    }
    catch (OperationCanceledException)
    {
        Console.WriteLine("Main task was cancelled.");
    }
});
