# Examples.DependencyInjection.Tests

## Project Initialize

```shell
dotnet new classlib -o src/Examples.DependencyInjection
dotnet sln add src/Examples.DependencyInjection/
cd src/Examples.DependencyInjection
dotnet add package Microsoft.Extensions.DependencyInjection
dotnet add package Microsoft.Extensions.Logging.Abstractions
cd ../../

dotnet new xunit -o src/Examples.DependencyInjection.Tests
dotnet sln add src/Examples.DependencyInjection.Tests/
cd src/Examples.DependencyInjection.Tests
dotnet add reference ../Examples.DependencyInjection
dotnet add package Microsoft.NET.Test.Sdk
dotnet add package xunit
dotnet add package xunit.runner.visualstudio
dotnet add package coverlet.collector
dotnet add package Moq
dotnet add package ChainingAssertion.Core.Xunit
cd ../../
```
