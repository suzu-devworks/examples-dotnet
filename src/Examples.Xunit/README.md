# Examples.Various.Tests

## Project Initialize

```shell
## Solution
dotnet new sln -o .

## Examples.Xunit
dotnet new classlib -o src/Examples.Xunit
dotnet sln add src/Examples.Xunit
cd src/Examples.Xunit
dotnet add package xunit
cd ../../

# Update outdated package
dotnet list package --outdated
```
