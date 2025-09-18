# Examples.DependencyInjection.Autofac.Tests

This is a project for learning Ioc using `Autofac`

## Table of Contents <!-- omit in toc -->

- [Examples.DependencyInjection.Autofac.Tests](#examplesdependencyinjectionautofactests)
  - [What is Autofac?](#what-is-autofac)
    - [Setup](#setup)
  - [References](#references)
  - [Development](#development)
    - [How the project was initialized](#how-the-project-was-initialized)

## What is Autofac?

Autofac is an addictive Inversion of Control container for .NET Core, ASP.NET Core, .NET 4.5.1+, Universal Windows apps, and more.

- <https://autofac.org/>

> for .NET Core, .NET Framework 4.5.1+

### Setup

```shell
dotnet add package Autofac
```

## References

## Development

### How the project was initialized

This project was initialized with the following command:

```shell
## Solution
dotnet new sln -o .

## Examples.DependencyInjection.Autofac.Tests
dotnet new xunit -o src/Examples.DependencyInjection.Autofac.Tests
dotnet sln add src/Examples.DependencyInjection.Autofac.Tests/
cd src/Examples.DependencyInjection.Autofac.Tests
dotnet add package Microsoft.NET.Test.Sdk
dotnet add package xunit
dotnet add package xunit.runner.visualstudio
dotnet add package coverlet.collector
dotnet add package Moq
dotnet add package ChainingAssertion.Core.Xunit
dotnet add package Autofac
cd ../../

# Update outdated package
dotnet list package --outdated
```
