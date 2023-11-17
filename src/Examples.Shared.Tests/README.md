# Examples.Shared.Tests

## Project Initialize

```shell
dotnet new classlib -o src/Examples.Shared
dotnet sln add src/Examples.Shared/
cd src/Examples.Shared
cd ../../

dotnet new xunit -o src/Examples.Shared.Tests
dotnet sln add src/Examples.Shared.Tests/
cd src/Examples.Shared.Tests
dotnet add reference ../Examples.Shared
dotnet add package Microsoft.NET.Test.Sdk
dotnet add package xunit
dotnet add package xunit.runner.visualstudio
dotnet add package coverlet.collector
dotnet add package Moq
dotnet add package ChainingAssertion.Core.Xunit
cd ../../
```
