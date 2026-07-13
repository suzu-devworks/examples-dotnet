#!/usr/bin/env dotnet run
#:package ConsoleAppFramework@5.*

// Chap07-03.cs
// [CancellationToken(Gracefully Shutdown) and Timeout](https://github.com/Cysharp/ConsoleAppFramework#cancellationtokengracefully-shutdown-and-timeout)
//
// In the case of `Run`/`RunAsync` from ConsoleAppBuilder, you can also pass a CancellationToken.
// This is combined with PosixSignalRegistration and passed to each method.
// This makes it possible to cancel at any arbitrary timing.

using ConsoleAppFramework;

// Create a CancellationTokenSource that will be cancelled when 'Q' is pressed.
var cts = new CancellationTokenSource();
_ = Task.Run(() =>
{
    while (Console.ReadKey().Key != ConsoleKey.Q) ;
    Console.WriteLine();
    cts.Cancel();
});

var app = ConsoleApp.Create();

app.Add("", async (CancellationToken cancellationToken) =>
{
    // CancellationToken will be triggered when 'Q' is pressed or Ctrl+C(SIGINT/SIGTERM) is sent.
    try
    {
        for (int i = 0; i < 10; i++)
        {
            Console.WriteLine($"Running main task iteration {i + 1}/10. Press 'Q' to quit.");
            await Task.Delay(TimeSpan.FromSeconds(1), cancellationToken);
        }
    }
    catch (OperationCanceledException)
    {
        Console.WriteLine("Main task was cancelled.");
    }
});

await app.RunAsync(args, cts.Token); // pass external CancellationToken
