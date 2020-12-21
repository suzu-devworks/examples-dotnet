using System;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

#pragma warning disable IDE0051

namespace Examples.DI.Core
{
    public class GenericsContainerTests
    {
        class GenericThing<T> : IThing<T>
        {
            public GenericThing()
            {
                Name = typeof(T).Name;
                Default = default;
            }

            public string Name { get; }

            public T Default { get; }
        }

        [Fact]
        void TestWithGeneric()
        {
            var services = new ServiceCollection();
            services.AddSingleton(typeof(IThing<>), typeof(GenericThing<>));

            using var provider = services.BuildServiceProvider();

            var dateTimeService1 = provider.GetService<IThing<DateTime>>();
            var dateTimeService2 = provider.GetService<IThing<DateTime>>();

            Assert.Same(dateTimeService1, dateTimeService2);

            var intService1 = provider.GetService<IThing<int>>();

            Assert.NotSame(dateTimeService1, intService1);
            Assert.NotSame(dateTimeService2, intService1);

            return;
        }

    }
}
