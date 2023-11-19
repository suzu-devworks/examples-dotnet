# Examples.Hosting.Console

## Project Initialize

```shell
dotnet new classlib -o src/Examples.Hosting
dotnet sln add src/Examples.Hosting/
cd src/Examples.Hosting
dotnet add package Microsoft.Extensions.Hosting.Abstractions
dotnet add package Microsoft.Extensions.Logging.Abstractions
cd ../../

dotnet new console -o src/Examples.Hosting.Console
dotnet sln add src/Examples.Hosting.Console
cd src/Examples.Hosting.Console
dotnet add package Microsoft.Extensions.Hosting
dotnet add reference ../Examples.Hosting
cd ../../
```
