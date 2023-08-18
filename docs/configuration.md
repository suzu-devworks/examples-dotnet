# Configuration

## The way to the present

```shell
git clone https://github.com/suzu-devworks/examples-dotnet.git
cd examples-dotnet

## run in Dev Container.

dotnet new sln -o .

## Examples.DependencyInjection
dotnet new classlib -o src/Examples.DependencyInjection
cd src/Examples.DependencyInjection
dotnet add package Microsoft.Extensions.DependencyInjection
dotnet add package Microsoft.Extensions.Logging.Abstractions
cd ../../
dotnet sln add src/Examples.DependencyInjection/

## Examples.DependencyInjection.Tests
dotnet new xunit -o src/Examples.DependencyInjection.Tests
cd src/Examples.DependencyInjection.Tests
dotnet add reference ../Examples.DependencyInjection
dotnet add package Moq
dotnet add package ChainingAssertion.Core.Xunit
cd ../../
dotnet sln add src/Examples.DependencyInjection.Tests/


dotnet build


# Update outdated package
dotnet list package --outdated

# Tools
dotnet new tool-manifest
dotnet tool install coverlet.console

```
