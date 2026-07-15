# Examples.Hosting.ConsoleAppFramework

## Overview and Purpose

This project is an application designed to learn how to implement logging with `ConsoleAppFramework` using `Microsoft.Extensions.Hosting`.

## What is ConsoleAppFramework?

ConsoleAppFramework is Zero Dependency, Zero Overhead, Zero Reflection, Zero Allocation,
AOT Safe CLI Framework powered by C# Source Generator.

Because it uses a source generator, you can create modern CLI applications with less code compared to other CLI frameworks.
Execution speed is extremely fast, as complex analysis is all excluded from the specification.

Furthermore, it supports DI (Dependency Injection), so it can be used in conjunction with `Microsoft.Extensions.Hosting`
and other DI containers.

- [ConsoleAppFramework](https://github.com/Cysharp/ConsoleAppFramework)

> Since .NET Standard 2.0

```bash
dotnet add package ConsoleAppFramework
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

### Getting Started scripts

We have implemented the contents of the topics including
[Getting Started](https://github.com/Cysharp/ConsoleAppFramework#getting-started) in the README of the official website.
Most of these samples include startup code, so it is difficult to implement them in a single Console app.
It is implemented as a "File-based apps" introduced in C# 14.

To do so, use a command similar to the following:

```bash
dotnet run --file Scripts/GettingStarted/Chap01/Chap01-01.cs -- --help
```
