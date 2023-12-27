# Examples.Metaprogramming.Tests

Metaprogramming is a programming technique in which computer programs have the ability to treat other programs as their data. 
It means that a program can be designed to read, generate, analyze or transform other programs, and even modify itself while running.

> [Wikipedia ...](https://en.wikipedia.org/wiki/Metaprogramming)

## Approaches

- [System.Reflection namespace ...](https://learn.microsoft.com/ja-jp/dotnet/api/system.reflection?view=net-8.0)
    > Contains types that retrieve information about assemblies, modules, members, parameters, and other entities in managed code by examining their metadata. 
- [System.Reflection.Emit namespace ...](https://learn.microsoft.com/ja-jp/dotnet/api/system.reflection.emit?view=net-8.0)
    > Contains classes that can output metadata and Microsoft Intermediate Language (MSIL) and optionally generate PE files on disk
- [System.Linq.Expressions namespace ...](https://learn.microsoft.com/ja-jp/dotnet/api/system.linq.expressions?view=net-8.0)
    > Contains classes, interfaces and enumerations that enable language-level code expressions to be represented as objects in the form of expression trees.
- [Microsoft.CodeAnalysis namespace ...](https://github.com/dotnet/roslyn)
    > Roslyn is the open-source implementation of both the C# and Visual Basic compilers with an API surface for building code analysis tools.
- [Mono.Cecil ...](https://www.mono-project.com/docs/tools+libraries/libraries/Mono.Cecil/)
    > Cecil is a library written by Jb Evain to generate and inspect programs and libraries in the ECMA CIL format.<br />
    > With Cecil, you can load existing managed assemblies, browse all the contained types, modify them on the fly and save back to the disk the modified assembly.


## Project Initialize

```shell
## Solution
dotnet new sln -o .

## Examples.Metaprogramming.Tests
dotnet new xunit -o src/Examples.Metaprogramming.Tests
dotnet sln add src/Examples.Metaprogramming.Tests

cd src/Examples.Metaprogramming.Tests
dotnet add reference ../../src/Examples.Xunit

dotnet add package Microsoft.NET.Test.Sdk
dotnet add package xunit
dotnet add package xunit.runner.visualstudio
dotnet add package coverlet.collector
dotnet add package Moq
dotnet add package ChainingAssertion.Core.Xunit

dotnet add package Mono.Cecil
cd ../../

# Update outdated package
dotnet list package --outdated
```
