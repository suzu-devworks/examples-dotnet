# examples-dotnet
Workspace for studying dotnet programming.


### The way to the present

```shell
git clone https://github.com/suzu-devworks/examples-dotnet.git
cd examples-dotnet

dotnet new sln -o .
dotnet new console -o src/Examples
dotnet sln add src/Examples/Examples.csproj
dotnet new xunit -o test/Examples.Tests
dotnet sln add test/Examples.Tests/Examples.Tests.csproj
dotnet add test/Examples.Tests/Examples.Tests.csproj reference src/Examples/Examples.csproj

## clear newline(CR).
find . -type d -name '.git' -prune -o -type f -print | xargs sed -i 's/\r//g'
## clear BOM(UTF-8).
find . -type d -name '.git' -prune -o -type f -print | xargs sed -i -s -e '1s/^\xef\xbb\xbf//'

## Package
dotnet add src/Examples package System.Text.Encoding.CodePages
dotnet add src/Examples package Mono.TextTemplating

dotnet add test/Examples.Tests package Moq
dotnet add test/Examples.Tests package ChainingAssertion.Core.Xunit

dotnet restore

# Tool
dotnet new tool-manifest
dotnet tool install dotnet-t4 --version 2.2.0
dotnet tool restore 

dotnet clean
dotnet build
```

### Referenced.

* https://docs.microsoft.com/ja-jp/dotnet/core/testing/unit-testing-with-dotnet-test

