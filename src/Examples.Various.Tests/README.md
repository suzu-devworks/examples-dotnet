# Examples.Various.Tests

## Project Initialize

```shell
## Solution
dotnet new sln -o .

## Examples.Various.Tests
dotnet new xunit -o tests/Examples.Various.Tests
dotnet sln add tests/Examples.Various.Tests/
cd tests/Examples.Various.Tests
dotnet add reference ../../src/Examples.Shared
dotnet add package Microsoft.NET.Test.Sdk
dotnet add package xunit
dotnet add package xunit.runner.visualstudio
dotnet add package coverlet.collector
dotnet add package Moq
dotnet add package ChainingAssertion.Core.Xunit

dotnet add package Microsoft.Extensions.Caching.Memory
dotnet add package System.Runtime.Caching
cd ../../

# Update outdated package
dotnet list package --outdated
```
