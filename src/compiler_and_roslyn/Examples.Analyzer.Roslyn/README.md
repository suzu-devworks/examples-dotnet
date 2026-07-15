# Examples.Analyzer.Roslyn

## Overview and Purpose

This project contains code for experimenting with and learning how to implement code analyzers using `Microsoft.CodeAnalysis`.

The .NET Compiler Platform SDK enables you to build **analyzers** and **code fixes** that find and correct coding mistakes.

## Project structure

- `Examples.Analyzer.Roslyn`: Main analyzer project.
- `Examples.Analyzer.Roslyn.CodeFixes`: Code-fix project for analyzers in `Examples.Analyzer.Roslyn`.
- `Examples.Analyzer.Roslyn.Tests`: Test project for analyzers and code fixes.
- `Examples.Analyzer.Roslyn.Demo`: Demo project showing real-world usage of analyzers and code fixes.

## Features

### Tutorial: Write your first analyzer and code fix

- [Tutorial: Write your first analyzer and code fix - C# | Microsoft Learn](https://learn.microsoft.com/en-us/dotnet/csharp/roslyn-sdk/tutorials/how-to-write-csharp-analyzer-code-fix)

This tutorial creates an analyzer that finds local variable declarations that could be declared
using the const modifier but aren't. The accompanying code fix modifies those declarations to add the const modifier.

- [MakeConstAnalyzer.cs](./Tutorials/MakeConstAnalyzer.cs)

## References

- [The .NET Compiler Platform SDK (Roslyn APIs) - C# | Microsoft Learn](https://learn.microsoft.com/ja-jp/dotnet/csharp/roslyn-sdk/)
- [Microsoft.CodeAnalysis Namespace](https://learn.microsoft.com/ja-jp/dotnet/api/microsoft.codeanalysis)
- [dotnet/samples - GitHub](https://github.com/dotnet/samples/tree/main/csharp/roslyn-sdk)
