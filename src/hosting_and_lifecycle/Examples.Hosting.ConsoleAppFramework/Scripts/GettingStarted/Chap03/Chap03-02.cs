#!/usr/bin/env dotnet run
#:package ConsoleAppFramework@5.*
#pragma warning disable CA1822 // Mark members as static

// Chap03-02.cs
// [Command](https://github.com/Cysharp/ConsoleAppFramework#command)
//
// With Add<T>, you can add multiple commands at once using a class-based approach.
// If you want to write document comments for multiple commands,
// this approach allows for cleaner code, so it is recommended.

using ConsoleAppFramework;

var app = ConsoleApp.Create();
app.Add<MyCommands>();
app.Run(args);

internal class MyCommands
{
    /// <summary>Root command test.</summary>
    /// <param name="msg">-m, Message to show.</param>
    [Command("")]
    public void Root(string msg) => Console.WriteLine(msg);

    /// <summary>Display message.</summary>
    /// <param name="msg">Message to show.</param>
    public void Echo(string msg) => Console.WriteLine(msg);

    /// <summary>Sum parameters.</summary>
    /// <param name="x">left value.</param>
    /// <param name="y">right value.</param>
    public void Sum(int x, int y) => Console.WriteLine(x + y);
}
