# Examples.Fluency.Tests

## Table of Contents <!-- omit in toc -->

- [Development](#development)
  - [How the project was initialized](#how-the-project-was-initialized)

## Development

### How the project was initialized

This project was initialized with the following command:

```shell
## Solution
dotnet new sln -o .

## Examples.Fluency
dotnet new classlib -o src/Examples.Fluency
dotnet sln add src/Examples.Fluency/
cd src/Examples.Fluency
cd ../../

## Examples.Fluency.Tests
dotnet new xunit -o src/Examples.Fluency.Tests
dotnet sln add src/Examples.Fluency.Tests/
cd src/Examples.Fluency.Tests
dotnet add reference ../../src/Examples.Fluency
dotnet add package Microsoft.NET.Test.Sdk
dotnet add package xunit.v3
dotnet add package xunit.runner.visualstudio
dotnet add package coverlet.collector
cd ../../

# Update outdated package
dotnet list package --outdated
```
