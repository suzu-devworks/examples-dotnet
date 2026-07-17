#!/usr/bin/env dotnet run
#:package ConsoleAppFramework@5.*

// Chap02-01.cs
// [Option aliases and Help,Version](https://github.com/Cysharp/ConsoleAppFramework#option-aliases-and-help-version)
//
// In ConsoleAppFramework, instead of using attributes,
// you can provide descriptions and aliases for functions by writing Document Comments.

using ConsoleAppFramework;

ConsoleApp.Run(args, Commands.Hello);

file static class Commands
{
    /// <summary>
    /// Display Hello.
    /// </summary>
    /// <param name="message">-m, Message to show.</param>
    public static void Hello(string message) => Console.Write($"Hello, {message}");
}
