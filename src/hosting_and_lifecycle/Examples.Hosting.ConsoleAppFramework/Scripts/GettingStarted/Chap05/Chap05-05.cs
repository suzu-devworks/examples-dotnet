#!/usr/bin/env dotnet run
#:package ConsoleAppFramework@5.*

// Chap05-05.cs
// [Double-dash escaping](https://github.com/Cysharp/ConsoleAppFramework#double-dash-escaping)
//
// Arguments after double-dash (--) can be received as escaped arguments without being parsed

using ConsoleAppFramework;

// NG: dotnet run --project foo.csproj -- --foo 100 --bar bazbaz
// OK: dotnet run --file Scripts/GettingStarted/Chap05/Chap05-05.cs -- run --project foo.csproj -- --foo 100 --bar bazbaz
var app = ConsoleApp.Create();
app.Add("run", (string project, ConsoleAppContext context) =>
{
    // run --project foo.csproj -- --foo 100 --bar bazbaz
    Console.WriteLine(string.Join(" ", context.Arguments));
    // --project foo.csproj
    Console.WriteLine(string.Join(" ", context.CommandArguments!));
    // --foo 100 --bar bazbaz
    Console.WriteLine(string.Join(" ", context.EscapedArguments!));
});

app.Run(args);

// spell-checker: words bazbaz
