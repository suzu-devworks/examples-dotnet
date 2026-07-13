#!/usr/bin/env dotnet run
#:package ConsoleAppFramework@5.*

// Chap02-02.cs
// [Option aliases and Help,Version](https://github.com/Cysharp/ConsoleAppFramework#option-aliases-and-help-version)
//
// When a default value exists, that value is displayed by default.

using ConsoleAppFramework;

ConsoleApp.Run(args, (Fruit myFruit = Fruit.Apple, [HideDefaultValue] Fruit myFruit2 = Fruit.Grape) => { });

internal enum Fruit
{
    Orange, Grape, Apple
}

