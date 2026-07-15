#!/usr/bin/env dotnet run
#:package ConsoleAppFramework@5.*
#pragma warning disable CA1822 // Mark members as static

// Chap04-01.cs
// [Disable Naming Conversion](https://github.com/Cysharp/ConsoleAppFramework#disable-naming-conversion)
//
// Command and option names are automatically converted to kebab-case by default.
// This conversion can be disabled at the assembly level.

using ConsoleAppFramework;

[assembly: ConsoleAppFrameworkGeneratorOptions(DisableNamingConversion = true)]

var app = ConsoleApp.Create();
app.Add<MyProjectCommand>();
app.Run(args);

internal class MyProjectCommand
{
    public void ExecuteCommand(string fooBarBaz)
    {
        Console.WriteLine(fooBarBaz);
    }
}
