using System;
using System.Diagnostics;

namespace Examples.Utilities
{
    public class FunctionMeasurer
    {
        private FunctionMeasurer() { }

        public string Name { get; init; }

        public static FunctionMeasurer As(string name)
        {
            return new FunctionMeasurer() { Name = name };
        }

        public T Run<T>(Func<T> runner)
        {
            var startRamUsage = Environment.WorkingSet;

            var sw = new Stopwatch();
            sw.Start();

            var result = runner();

            sw.Stop();

            var endRamUsage = Environment.WorkingSet;
            GC.Collect();

            Console.WriteLine($"{Name,-30}: Elapsed = {sw.Elapsed,20}, RamUsage = {endRamUsage,20:N}, RamIncreased = {(endRamUsage - startRamUsage),20:N}");

            return result;
        }

    }
}
