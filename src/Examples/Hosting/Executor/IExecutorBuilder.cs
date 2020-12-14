using System;

namespace Examples.Hosting.Executor
{
    internal interface IExecutorBuilder
    {
        IHostedService Build();

        IExecutorBuilder AddRunner<T>(Action<T> action);

        IExecutorBuilder UseAttribute<T>();

        IExecutorBuilder UseExcludeAttribute<T>();

        IExecutorBuilder MeasureRamUsage(bool measures);

        IExecutorBuilder MeasureTimes(bool measures);
    }
}