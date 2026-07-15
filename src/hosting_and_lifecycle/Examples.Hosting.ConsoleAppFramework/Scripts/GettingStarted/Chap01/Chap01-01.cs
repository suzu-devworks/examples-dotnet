#!/usr/bin/env dotnet run
#:package ConsoleAppFramework@5.*

// Chap01-01.cs
// [Getting Started](https://github.com/Cysharp/ConsoleAppFramework#getting-started)
//
// The second argument of Run or RunAsync can be any lambda expression,
// method, or function reference. Based on the content of the second argument,
// the corresponding function is automatically generated.

using ConsoleAppFramework;

ConsoleApp.Run(args, (string name) => Console.WriteLine($"Hello {name}"));
