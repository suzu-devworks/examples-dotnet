# Examples.DependencyInjection.Tests

This is a test project for [Examples.DependencyInjection](../Examples.DependencyInjection/README.md).

## Table of Contents <!-- omit in toc -->

- [Examples.DependencyInjection.Tests](#examplesdependencyinjectiontests)
  - [Development](#development)
    - [How the project was initialized](#how-the-project-was-initialized)

## Development

### How the project was initialized

This project was initialized with the following command:

```shell
## Solution
dotnet new sln -o .

## Examples.DependencyInjection
dotnet new classlib -o src/Examples.DependencyInjection
dotnet sln add src/Examples.DependencyInjection/
cd src/Examples.DependencyInjection
dotnet add package Microsoft.Extensions.DependencyInjection
dotnet add package Microsoft.Extensions.Logging.Abstractions
cd ../../

## Examples.DependencyInjection.Tests
dotnet new xunit -o src/Examples.DependencyInjection.Tests
dotnet sln add src/Examples.DependencyInjection.Tests/
cd src/Examples.DependencyInjection.Tests
dotnet add package Microsoft.NET.Test.Sdk
dotnet add package xunit
dotnet add package xunit.runner.visualstudio
dotnet add package coverlet.collector
dotnet add package Moq
dotnet add package ChainingAssertion.Core.Xunit
cd ../../

# Update outdated package
dotnet list package --outdated
```
