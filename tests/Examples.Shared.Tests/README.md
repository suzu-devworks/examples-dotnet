# Examples.Shared.Tests

## Project Initialize

```shell
## Solution
dotnet new sln -o .

## Examples.Shared
dotnet new classlib -o src/Examples.Shared
dotnet sln add src/Examples.Shared/
cd src/Examples.Shared
cd ../../

## Examples.Shared.Tests
dotnet new xunit -o tests/Examples.Shared.Tests
dotnet sln add tests/Examples.Shared.Tests/
cd tests/Examples.Shared.Tests
dotnet add reference ../../src/Examples.Shared
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
