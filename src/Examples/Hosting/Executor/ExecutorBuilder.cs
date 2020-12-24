using System;
using System.Collections.Generic;

namespace Examples.Hosting.Executor
{
    internal class ExecutorBuilder : IExecutorBuilder
    {
        public IHostedService Build()
        {
            var service = new ExecutorService
            {
                Properties = this.Properties,
                Runners = this.Runners,
            };

            return service;
        }

        public IExecutorBuilder AddRunner<T>(Action<T> action)
        {
            this.Runners.Add(new(typeof(T), o => action((T)o)));
            return this;
        }

        public IExecutorBuilder MeasureRamUsage(bool measures)
        {
            var key = "MeasureRamUsage";
            if (!this.Properties.ContainsKey(key))
            {
                this.Properties.Add(key, false);
            }
            this.Properties[key] = measures;
            return this;
        }

        public IExecutorBuilder MeasureTimes(bool measures)
        {
            var key = "MeasureTimes";
            if (!this.Properties.ContainsKey(key))
            {
                this.Properties.Add(key, false);
            }
            this.Properties[key] = measures;
            return this;
        }

        public IExecutorBuilder UseExcludeAttribute<T>()
        {
            var key = "ExcludeAttributes";
            if (!this.Properties.ContainsKey(key))
            {
                this.Properties.Add(key, new HashSet<Type>());
            }
            var sets = this.Properties[key] as ISet<Type>;
            sets.Add(typeof(T));
            return this;
        }

        public IExecutorBuilder UseAttribute<T>()
        {
            var key = "UseAttribute";
            if (!this.Properties.ContainsKey(key))
            {
                this.Properties.Add(key, new HashSet<Type>());
            }
            var sets = this.Properties[key] as ISet<Type>;
            sets.Add(typeof(T));
            return this;
        }

        private readonly List<Tuple<Type, Action<object>>> Runners = new();

        private readonly Dictionary<object, object> Properties = new();

    }
}
