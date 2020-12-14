using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace Examples.Hosting.Executor
{
    internal class ExecutorService : IHostedService
    {
        public Task StartAsync(CancellationToken cancellationToken)
        {
            foreach (var (type, action) in Runners)
            {
                var concreteTypes = GetConcreteTypes(type);

                var concreteAction = GetAction(action);

                foreach (var concreteType in concreteTypes)
                {
                    Console.WriteLine($"Execute -> {concreteType.FullName}");
                    var instance = Activator.CreateInstance(concreteType);

                    concreteAction(instance);
                }
            }

            return Task.CompletedTask;
        }

        private IEnumerable<Type> GetConcreteTypes(Type type)
        {
            var types = (type.IsInterface)
                ? Assembly.GetExecutingAssembly().GetTypes()
                    .Where(t => t.GetInterfaces().Any(i => i == type))
                : new[] { type }.AsEnumerable();

            var runnerTypes = from t in types
                              where IsTargetRunner(t)
                              select t
                            ;

            return runnerTypes;
        }

        private bool IsTargetRunner(Type type)
        {
            var includeAttributes = GetPropertySafe<ISet<Type>>("UseAttribute");
            if (includeAttributes != null)
            {
                var hasIncludeAttributes = type.GetCustomAttributes(false)
                            .Any(a => includeAttributes.Contains(a.GetType()));
                return hasIncludeAttributes;
            }

            var excludeAttributes = GetPropertySafe<ISet<Type>>("ExcludeAttributes");
            if (excludeAttributes != null)
            {
                var hasExcludeAttributes = type.GetCustomAttributes(false)
                            .Any(a => excludeAttributes.Contains(a.GetType()));
                return (!hasExcludeAttributes);
            }

            return true;
        }

        private Action<object> GetAction(Action<object> baseAction)
        {
            var result = baseAction;

            if (GetPropertySafe<bool>("MeasureTimes"))
            {
                var newAction = DoMeasureTimes(result);
                result = newAction;
            }

            if (GetPropertySafe<bool>("MeasureRamUsage"))
            {
                var newAction = DoMeasureRamUsage(result);
                result = newAction;
            }

            return result;
        }

        private T GetPropertySafe<T>(string key)
        {
            return (this.Properties.ContainsKey(key))
                ? (T)this.Properties[key]
                : default(T);
        }

        private Action<object> DoMeasureTimes(Action<object> action)
        {
            return o =>
            {
                var sw = new Stopwatch();
                sw.Start();

                action(o);

                sw.Stop();
                Console.WriteLine($"{o.GetType().FullName,-30}: Elapsed  = {sw.Elapsed,20}");
            };
        }

        private Action<object> DoMeasureRamUsage(Action<object> action)
        {
            return o =>
            {
                var startRamUsage = Environment.WorkingSet;

                action(o);

                var endRamUsage = Environment.WorkingSet;
                GC.Collect();

                Console.WriteLine($"{o.GetType().FullName,-30}: RamUsage = {endRamUsage,20:N}, RamIncreased = {(endRamUsage - startRamUsage),20:N}");
            };
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public IDictionary<object, object> Properties { get; init; }

        public IEnumerable<Tuple<Type, Action<object>>> Runners { get; init; }

    }
}
