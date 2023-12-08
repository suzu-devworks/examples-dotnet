# Examples.Metaprogramming.Tests

## Project Initialize

```shell
## Solution
dotnet new sln -o .

## Examples.Metaprogramming.Tests
dotnet new xunit -o src/Examples.Metaprogramming.Tests
dotnet sln add src/Examples.Metaprogramming.Tests

cd src/Examples.Metaprogramming.Tests
dotnet add reference ../../src/Examples.Xunit

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
