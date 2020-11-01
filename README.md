# examples-dotnet
Workspace for studying dotnet programming.


### The way to the present

```shell
git clone https://github.com/suzu-devworks/examples-dotnet.git
cd examples-dotnet

dotnet new sln -o .
dotnet new console -o Examples
dotnet sln add ./Examples/Examples.csproj
dotnet new xunit -o Examples.Tests
dotnet sln add ./Examples.Tests/Examples.Tests.csproj
dotnet add ./Examples.Tests/Examples.Tests.csproj reference ./Examples/Examples.csproj
dotnet build
```

### referenced.

* https://docs.microsoft.com/ja-jp/dotnet/core/testing/unit-testing-with-dotnet-test

