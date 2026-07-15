#!/usr/bin/env dotnet run
#:package ConsoleAppFramework@5.*

// Chap03-01.cs
// [Command](https://github.com/Cysharp/ConsoleAppFramework#command)
//
// If you want to define a complex command,
// create an application with ConsoleApp.Create() and add the command with the Add method.

using ConsoleAppFramework;

var app = ConsoleApp.Create();

app.Add("", (string msg) => Console.WriteLine(msg));
app.Add("echo", (string msg) => Console.WriteLine(msg));
app.Add("sum", (int x, int y) => Console.WriteLine(x + y));

// --msg
// echo --msg
// sum --x --y

app.Run(args);
