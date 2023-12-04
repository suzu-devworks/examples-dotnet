# Examples.Benchmark

[BenchmarkDotNet](https://github.com/dotnet/BenchmarkDotNet)

## Project Initialize

```shell
## Solution
dotnet new sln -o .

## Examples.Benchmark
dotnet new console -o src/Examples.Benchmark
dotnet sln add src/Examples.Benchmark/
cd src/Examples.Benchmark
dotnet add package BenchmarkDotNet
cd ../../

# Update outdated package
dotnet list package --outdated
```

