#!/usr/bin/env dotnet run
#:package ConsoleAppFramework@5.*

// Chap01-04.cs
// [Getting Started](https://github.com/Cysharp/ConsoleAppFramework#getting-started)
//
// When defining an asynchronous method using a lambda expression,
// the `async` keyword is required.

using ConsoleAppFramework;

await ConsoleApp.RunAsync(args, async (int foo, int bar, CancellationToken cancellationToken) =>
{
    await Task.Delay(TimeSpan.FromSeconds(5), cancellationToken);
    Console.WriteLine($"Sum: {foo + bar}");
});
