#!/usr/bin/env dotnet run
#:package ConsoleAppFramework@5.*

// Chap03-07.cs
// [Alias command](https://github.com/Cysharp/ConsoleAppFramework#alias-command)
//
// Nested commands can also have aliases.

using ConsoleAppFramework;

var app = ConsoleApp.Create();

app.Add("dotnet package add|dotnet add package", () => { });
app.Add("dotnet package list|dotnet list package", () => { });
app.Add("dotnet package remove|dotnet remove package", () => { });
app.Add("dotnet reference add|dotnet add reference", () => { });

app.Run(args);
