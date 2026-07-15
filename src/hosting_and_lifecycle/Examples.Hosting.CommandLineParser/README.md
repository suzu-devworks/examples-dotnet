# Examples.Hosting.CommandLineParser

## Overview and Purpose

This project is an application designed to learn how to implement logging with `CommandLineParser` using `Microsoft.Extensions.Hosting`.

## What is CommandLineParser?

The Command Line Parser Library offers CLR applications a clean and concise API for manipulating command line arguments
 and related tasks, such as defining switches, options and verb commands.

The most noteworthy point is that the target framework is `netstandard2.0;net40;net45;net461`.
This makes it usable even in fairly old .NET Framework 4 projects.

Metaprogramming with attributes may seem a bit old-fashioned,
but it provides sufficient functionality for handling simple command-line arguments.

> [!IMPORTANT]
> There have been no releases since 2022,
and it seems there's a large backlog of unresolved issues and pull requests, which may pose implementation risks.

- [CommandLineParser](https://github.com/commandlineparser/commandline)

> Since .NET Framework 4.0

```bash
dotnet add package CommandLineParser
```

## Build

**Prerequisites**:

- **.NET SDK**: A version of `TargetFramework` that is supported is required.
See [LatestFramework](../Directory.Build.props) property.

There are no particular points to note during the build process.

Just run the commands as you normally would:

```bash
dotnet build
```

## Usage

Since this is a command line sample, let's take a look at the help first:

```bash
dotnet run -- --help
```

### ``quick`` command

This command is a custom implementation of the content in
the [Quick Start Examples](https://github.com/commandlineparser/commandline#quick-start-examples)
using `Microsoft.Extensions.Hosting`.

```bash
dotnet run -- quick --help
```

### ``syntax``, ``syntax-options``, ``syntax-exclusive``, ``syntax-grouping`` command

These commands are for learning the syntax of command-line arguments. They basically refer to
the [Wiki - commandlineparser/commandline](https://github.com/commandlineparser/commandline/wiki),
but I implemented them myself to verify their actual behavior.

```bash
dotnet run -- syntax --help
dotnet run -- syntax-options --help
dotnet run -- syntax-exclusive --help
dotnet run -- syntax-grouping --help
```

## Cannot implement nested subcommands

The CommandLineParser library does not support nested subcommands, so it is not suitable for complex command structures.
However, I don't think there are many such requirements.

I think that once you use it, you won't be able to do anything too complicated.
