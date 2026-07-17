#!/usr/bin/env dotnet run
#:package ConsoleAppFramework@5.*

// Chap03-03.cs
// [Nested command](https://github.com/Cysharp/ConsoleAppFramework#nested-command)
//
// You can define a hierarchical structure of commands by separating paths with spaces.

using ConsoleAppFramework;

var app = ConsoleApp.Create();

app.Add("foo", () => { });
app.Add("foo bar", () => { });
app.Add("foo bar barbaz", () => { });
app.Add("foo baz", () => { });

// Commands:
//   foo
//   foo bar
//   foo bar barbaz
//   foo baz

app.Run(args);
