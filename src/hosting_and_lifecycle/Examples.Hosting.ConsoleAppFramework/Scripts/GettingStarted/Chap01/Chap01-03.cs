#!/usr/bin/env dotnet run
#:package ConsoleAppFramework@5.*
#:property AllowUnsafeBlocks=true

// Chap01-03.cs
// [Getting Started](https://github.com/Cysharp/ConsoleAppFramework#getting-started)
//
// For static functions, you can pass them as function pointers.
// In that case, the managed function pointer arguments will be generated,
// resulting in maximum performance.

using ConsoleAppFramework;

unsafe
{
    ConsoleApp.Run(args, &Sum);
}

static void Sum(int x, int y) => Console.Write(x + y);
