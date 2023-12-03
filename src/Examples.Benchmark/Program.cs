using BenchmarkDotNet.Running;

Console.WriteLine("Start Benchmark");
BenchmarkSwitcher.FromAssembly(typeof(Program).Assembly).Run(args);
Console.WriteLine("Finished.");
