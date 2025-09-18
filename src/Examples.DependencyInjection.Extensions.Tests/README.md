# Examples.DependencyInjection.Extensions.Tests

This is a project for learning Ioc using `Microsoft.Extensions.DependencyInjection`

## Table of Contents <!-- omit in toc -->

- [Examples.DependencyInjection.Extensions.Tests](#examplesdependencyinjectionextensionstests)
  - [What is Microsoft.Extensions.DependencyInjection?](#what-is-microsoftextensionsdependencyinjection)
    - [Setup](#setup)
  - [References](#references)
  - [Development](#development)
    - [How the project was initialized](#how-the-project-was-initialized)

## What is Microsoft.Extensions.DependencyInjection?

Provides classes that support the implementation of the dependency injection software design pattern.

> Since .NET Platform Extensions 1.0

### Setup

```shell
dotnet add package Microsoft.Extensions.DependencyInjection
```

## References

- [.NET dependency injection - Microsoft Learn](https://learn.microsoft.com/ja-jp/dotnet/core/extensions/dependency-injection)
- [Dependency injection in ASP.NET Core - Microsoft Learn](https://docs.microsoft.com/ja-jp/aspnet/core/fundamentals/dependency-injection)
- [Microsoft.Extensions.DependencyInjection Namespace - Microsoft Learn](https://docs.microsoft.com/ja-jp/dotnet/api/microsoft.extensions.dependencyinjection)

## Development

### How the project was initialized

This project was initialized with the following command:

```shell
## Solution
dotnet new sln -o .

## Examples.DependencyInjection.Extensions.Tests
dotnet new xunit -o src/Examples.DependencyInjection.Extensions.Tests
dotnet sln add src/Examples.DependencyInjection.Extensions.Tests/
cd src/Examples.DependencyInjection.Extensions.Tests
dotnet add package Microsoft.NET.Test.Sdk
dotnet add package xunit
dotnet add package xunit.runner.visualstudio
dotnet add package coverlet.collector
dotnet add package Moq
dotnet add package ChainingAssertion.Core.Xunit
dotnet add package Microsoft.Extensions.DependencyInjection
dotnet add package Microsoft.Extensions.Logging
cd ../../

# Update outdated package
dotnet list package --outdated
```
