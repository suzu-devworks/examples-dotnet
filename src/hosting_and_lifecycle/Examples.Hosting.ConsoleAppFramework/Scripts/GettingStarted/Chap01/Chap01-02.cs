#!/usr/bin/env dotnet run
#:package ConsoleAppFramework@5.*

// Chap01-02.cs
// [Getting Started](https://github.com/Cysharp/ConsoleAppFramework#getting-started)
//
// When passing a method, you can write it as follows.

using ConsoleAppFramework;

ConsoleApp.Run(args, Sum);

void Sum(int x, int y) => Console.Write(x + y);
