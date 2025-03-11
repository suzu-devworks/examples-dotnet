# Examples.DependencyInjection.IoC.Tests

## IoC Containers

- [Microsoft.Extensions.DependencyInjection ...](./DependencyInjection.Tests/)
- [Autofac IoC container ...](./DependencyInjection.Autofac.Tests/)
- [MEF2 (.NET 4.5)  ...](./DependencyInjection.Mef2.Tests/)
- [MEF (.NET 4.5)  ...](./DependencyInjection.Mef1.Registration.Tests/)
- [MEF (.NET 4.0) ...](./DependencyInjection.Mef1.Tests/)

## Project Initialize

```shell
## Solution
dotnet new sln -o .

## Examples.DependencyInjection.IoC.Tests
dotnet new xunit -o src/Examples.DependencyInjection.IoC.Tests
dotnet sln add src/Examples.DependencyInjection.IoC.Tests/
cd src/Examples.DependencyInjection.Tests
dotnet add package Microsoft.NET.Test.Sdk
dotnet add package xunit
dotnet add package xunit.runner.visualstudio
dotnet add package coverlet.collector
dotnet add package Moq
dotnet add package ChainingAssertion.Core.Xunit

dotnet add package Microsoft.Extensions.DependencyInjection
dotnet add package System.ComponentModel.Composition
dotnet add package System.ComponentModel.Composition.Registration
dotnet add package System.Composition
dotnet add package Autofac
cd ../../

# Update outdated package
dotnet list package --outdated
```
