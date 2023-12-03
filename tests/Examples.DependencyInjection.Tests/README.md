# Examples.DependencyInjection.Tests

## Project Initialize

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
dotnet new xunit -o tests/Examples.DependencyInjection.Tests
dotnet sln add tests/Examples.DependencyInjection.Tests/
cd tests/Examples.DependencyInjection.Tests
dotnet add reference ../../src/Examples.DependencyInjection
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
