using System.Threading;

namespace Examples.Hosting.Executor
{
    internal static class ExecutorServiceExtension
    {
        public static void Run(this IHostedService service)
        {
            service.StartAsync(default(CancellationToken)).GetAwaiter().GetResult();
        }
    }
}