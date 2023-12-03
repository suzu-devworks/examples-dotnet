# Examples.Hosting.Workers

## Project Initialize

```shell
## Solution
dotnet new sln -o .

## Examples.Hosting.Workers
dotnet new worker -o src/Examples.Hosting.Workers
dotnet sln add src/Examples.Hosting.Workers/
cd src/Examples.Hosting.Workers
dotnet add reference ../Examples.Hosting
cd ../../

# Update outdated package
dotnet list package --outdated
```
