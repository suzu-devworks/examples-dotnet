# Examples.CodeGenerator.Roslyn

## Overview and Purpose

This project provides a code generator that can generate C# code at compile time using the Roslyn API.

This generator is primarily intended for learning and testing purposes,
so it may contain some imperfections. Please be aware of this.

Initially, I intended to learn how to use all the generators,
but currently, creating a generator using `IGenerator` in C# 9.0 results in an error,
so I've decided to focus on `IIncrementalGenerator` in C# 10.0.

## Project structure

- `Examples.CodeGenerator.Roslyn`: The main project that contains the code generator implementation.
- `Examples.CodeGenerator.Roslyn.Tests`: A test project for verifying the behavior of the code generation functionality.
- `Examples.CodeGenerator.Roslyn.Demo`: A demo project that showcases the usage of the code generator in a real-world scenario.

## Features

### Introducing C# Source Generators

The original code was for `IGenerator`, but I managed to change it to `IIncrementalGenerator`.

- [See Generators](./Introducing/)

### Generator recipes

I create generators based on ideas I might have or that I think could actually be useful.

- [EnumDescriptionGenerator](./Recipes/EnumDescriptionGenerator.cs)

  > This creates an extension method to retrieve the "Description" of an enumeration type.
  The "Description" is obtained from an XML comment.
  Don't forget to set `<GenerateDocumentationFile>` in your `*.csproj` file.

- [NotifyPropertyChangedGenerator](./)

  > This is a common example in source code generators.
  When you hear `INotifyPropertyChanged`,
  you probably think, "Oh, right." It's really annoying, isn't it?

## Development Notes

### How can I use the latest C# features with .NET Standard 2.0?

#### Use `record` / `init`

The following error may occur:

```console
CS0518: Predefined type 'System.Runtime.CompilerServices.IsExternalInit' is not defined or imported
```

To solve it:

- Define `System.Runtime.CompilerServices.IsExternalInit` class.

#### Use `required`

To use:

- Define `System.Runtime.CompilerServices.RequiredMemberAttribute` with `AttributeTargets.Property | AttributeTargets.Field`
- Define `System.Runtime.CompilerServices.SetsRequiredMembersAttribute` with `AttributeTargets.Constructor`

### What exactly is EmbeddedAttribute?

In .NET 10.0, a method called `AddEmbeddedAttributeDefinition()` was added to `IncrementalGeneratorPostInitializationContext`.
This method allows you to add an `EmbeddedAttribute` to a project that references a generator.

In short, `Microsoft.CodeAnalysis.EmbeddedAttribute` is an attribute that the compiler references and hides from
external access.

When generating attributes as `internal` and sharing access via `InternalsVisibleTo`,
conflicts can occur when multiple projects reference the same generator.

For example, you may encounter `warning CS0436`, indicating conflicting `*Attribute` types.

Applying this attribute helps prevent those conflicts.

By applying it to source-generated attributes, each project can keep its generated attributes compiler-internal.

In earlier versions such as .NET 9.0, managing generated attributes appears to have been more error-prone.

## References

- [C# Source Generators | Microsoft Learn](https://learn.microsoft.com/ja-jp/shows/on-dotnet/c-source-generators)
- [Incremental Generators - dotnet/roslyn](https://github.com/dotnet/roslyn/blob/main/docs/features/incremental-generators.md)
- [Incremental Generators Cookbook - dotnet/roslyn](https://github.com/dotnet/roslyn/blob/main/docs/features/incremental-generators.cookbook.md)
