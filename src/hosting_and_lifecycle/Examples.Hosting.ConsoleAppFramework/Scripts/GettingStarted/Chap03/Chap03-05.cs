#!/usr/bin/env dotnet run
#:package ConsoleAppFramework@5.*
#pragma warning disable CA1822 // Mark members as static

// Chap03-05.cs
// [Register from attribute](https://github.com/Cysharp/ConsoleAppFramework#register-from-attribute)
//
// `RegisterCommands` attribute can be used to automatically register
// commands without explicitly calling the `Add` method.

using ConsoleAppFramework;

var app = ConsoleApp.Create();

// Commands:
//   baz
//   bar baz

app.Run(args);

[RegisterCommands]
internal class Foo
{
    public void Baz(int x)
    {
        Console.Write(x);
    }
}

[RegisterCommands("bar")]
internal class Bar
{
    public void Baz(int x)
    {
        Console.Write(x);
    }
}
