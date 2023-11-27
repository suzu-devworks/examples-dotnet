# Examples.Hosting.Workers

## Project Initialize

```shell
dotnet new classlib -o src/Examples.Hosting
dotnet sln add src/Examples.Hosting/
cd src/Examples.Hosting
dotnet add package Microsoft.Extensions.Hosting.Abstractions
dotnet add package Microsoft.Extensions.Logging.Abstractions
cd ../../

dotnet new worker -o src/Examples.Hosting.Workers
dotnet sln add src/Examples.Hosting.Workers/
cd src/Examples.Hosting.Workers
dotnet add reference ../Examples.Hosting
cd ../../
```
