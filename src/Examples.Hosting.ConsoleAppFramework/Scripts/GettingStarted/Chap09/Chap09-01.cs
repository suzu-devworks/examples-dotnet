#!/usr/bin/env dotnet run
#:package ConsoleAppFramework@5.*

// Chap09-01.cs
// [Attribute based parameters validation](https://github.com/Cysharp/ConsoleAppFramework#attribute-based-parameters-validation)
//
// `ConsoleAppFramework` performs validation when the parameters are marked with attributes
// for validation from `System.ComponentModel.DataAnnotations`.

using System.ComponentModel.DataAnnotations;
using ConsoleAppFramework;

ConsoleApp.Run(args, ([EmailAddress] string firstArg, [Range(0, 2)] int secondArg) => { });
