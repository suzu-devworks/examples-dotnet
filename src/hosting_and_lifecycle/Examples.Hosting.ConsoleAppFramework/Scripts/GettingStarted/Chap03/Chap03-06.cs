#!/usr/bin/env dotnet run
#:package ConsoleAppFramework@5.*
#pragma warning disable CA1822 // Mark members as static

// Chap03-06.cs
// [Alias command](https://github.com/Cysharp/ConsoleAppFramework#alias-command)
//
// In the Add method, separating commandName or [Command] with | defines them as aliases.

using ConsoleAppFramework;

var app = ConsoleApp.Create();

app.Add("build|b", () => { });
app.Add("keyvault|kv", () => { });
app.Add<Commands>();

app.Run(args);

internal class Commands
{
    /// <summary>Executes the check command using the specified coordinates.</summary>
    [Command("check|c")]
    public void Check() { }

    /// <summary>Build this packages's and its dependencies' documentation.</summary>
    [Command("doc|d")]
    public void Doc() { }
}
