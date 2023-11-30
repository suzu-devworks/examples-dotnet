# Examples.Serialization.Tests

## Project Initialize

```shell
## Solution
dotnet new sln -o .

## Examples.Serialization.Tests
dotnet new xunit -o src/Examples.Serialization.Tests
cd src/Examples.Serialization.Tests
dotnet add reference ../Examples.Serialization
dotnet add package Microsoft.NET.Test.Sdk
dotnet add package xunit
dotnet add package xunit.runner.visualstudio
dotnet add package coverlet.collector
dotnet add package Moq
dotnet add package ChainingAssertion.Core.Xunit
dotnet add package YamlDotNet
dotnet add package Google.Protobuf
dotnet add package Grpc.Tools

# Update outdated package
dotnet list package --outdated
```
