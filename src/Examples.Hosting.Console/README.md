# Examples.Hosting.Console

## Project Initialize

```shell
## Solution
dotnet new sln -o .

## Examples.Hosting.Console
dotnet new console -o src/Examples.Hosting.Console
dotnet sln add src/Examples.Hosting.Console
cd src/Examples.Hosting.Console
dotnet add package Microsoft.Extensions.Hosting
dotnet add package System.CommandLine --prerelease
cd ../../

# Update outdated package
dotnet list package --outdated
```
