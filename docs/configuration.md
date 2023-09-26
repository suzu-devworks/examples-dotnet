# Configuration

## The way to the present

```shell
git clone https://github.com/suzu-devworks/examples-dotnet.git
cd examples-dotnet

## run in Dev Container.

dotnet new sln -o .

#dotnet nuget update source github --username suzu-devworks --password "{parsonal access token}" --store-password-in-clear-text

## Examples.Shared
dotnet new classlib -o src/Examples.Shared
dotnet sln add src/Examples.Shared/
cd src/Examples.Shared
cd ../../

## Examples.Shared.Tests
dotnet new xunit -o src/Examples.Shared.Tests
dotnet sln add src/Examples.Shared.Tests/
cd src/Examples.Shared.Tests
dotnet add reference ../Examples.Shared
dotnet add package Moq
dotnet add package ChainingAssertion.Core.Xunit
cd ../../

## Examples.DependencyInjection
dotnet new classlib -o src/Examples.DependencyInjection
dotnet sln add src/Examples.DependencyInjection/
cd src/Examples.DependencyInjection
dotnet add package Microsoft.Extensions.DependencyInjection
dotnet add package Microsoft.Extensions.Logging.Abstractions
cd ../../

## Examples.DependencyInjection.Tests
dotnet new xunit -o src/Examples.DependencyInjection.Tests
dotnet sln add src/Examples.DependencyInjection.Tests/
cd src/Examples.DependencyInjection.Tests
dotnet add reference ../Examples.DependencyInjection
dotnet add package Moq
dotnet add package ChainingAssertion.Core.Xunit
cd ../../

## Examples.Hosting
dotnet new classlib -o src/Examples.Hosting
dotnet sln add src/Examples.Hosting/
cd src/Examples.Hosting
dotnet add package Microsoft.Extensions.Hosting.Abstractions
dotnet add package Microsoft.Extensions.Logging.Abstractions
cd ../../

## Examples.Hosting.Workers
dotnet new worker -o src/Examples.Hosting.Workers
dotnet sln add src/Examples.Hosting.Workers/
cd src/Examples.Hosting.Workers
dotnet add reference ../Examples.Hosting
cd ../../

dotnet build


# Update outdated package
dotnet list package --outdated

# Tools
dotnet new tool-manifest
dotnet tool install coverlet.console

```
