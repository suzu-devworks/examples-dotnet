# Examples.Serialization.Tests

## Project Initialize

```shell
## Solution
dotnet new sln -o .

## Examples.Shared
dotnet new classlib -o src/Examples.Serialization
dotnet sln add src/Examples.Serialization/
cd src/Examples.Serialization
dotnet add package YamlDotNet
dotnet add package Google.Protobuf
dotnet add package Grpc.Tools
cd ../../

## Examples.Serialization.Tests
dotnet new xunit -o tests/Examples.Serialization.Tests
dotnet sln add tests/Examples.Serialization.Tests/
cd tests/Examples.Serialization.Tests
dotnet add reference ../Examples.Serialization/
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
