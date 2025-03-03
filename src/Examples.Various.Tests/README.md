# Examples.Various.Tests

## Table of Contents <!-- omit in toc -->

- [Examples.Various.Tests](#examplesvarioustests)
  - [Development](#development)
    - [How the project was initialized](#how-the-project-was-initialized)

## Development

### How the project was initialized

This project was initialized with the following command:

```shell
## Solution
dotnet new sln -o .

## Examples.Various.Tests
dotnet new xunit -o src/Examples.Various.Tests
dotnet sln add src/Examples.Various.Tests/
cd src/Examples.Various.Tests
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
