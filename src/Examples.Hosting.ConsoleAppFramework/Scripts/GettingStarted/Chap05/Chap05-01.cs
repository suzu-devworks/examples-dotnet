#!/usr/bin/env dotnet run
#:package ConsoleAppFramework@5.*

// Chap05-01.cs
// [Parse and Value Binding](https://github.com/Cysharp/ConsoleAppFramework#parse-and-value-binding)
//
// The method parameter names and types determine how to parse and bind values
// from the command-line arguments.

using ConsoleAppFramework;

ConsoleApp.Run(args, (
    [Argument] DateTime dateTime,   // Argument
    [Argument] Guid guidvalue,      //
    int intVar,                     // required
    bool boolFlag,                  // flag
    MyEnum enumValue,               // enum
    int[] array,                    // array
    MyClass obj,                    // object
    string optional = "abcde",      // optional
    double? nullableValue = null,   // nullable
    params string[] paramsArray     // params
    ) =>
{ });

internal enum MyEnum
{
    A, B, C
}

internal class MyClass
{
    public string Name { get; set; } = default!;
    public int Age { get; set; }
}

// spell-checker: words guidvalue
