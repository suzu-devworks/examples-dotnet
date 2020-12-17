using System;
using Examples.Hosting.Executor;

namespace Examples
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            new ExecutorBuilder()
                .UseExcludeAttribute<ExcludeAttribute>()
                .MeasureTimes(true)
                .MeasureRamUsage(false)
                .AddRunner<IRunner>(x => x.Run())
                .Build()
                .Run();

            return;
        }
    }
}
