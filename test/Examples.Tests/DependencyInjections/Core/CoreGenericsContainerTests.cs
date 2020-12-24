using System;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

#pragma warning disable IDE0051

namespace Examples.DependencyInjections.Core
{
    public class CoreGenericsContainerTests
    {
        class Provider
        { }

        interface IThing<T>
        {
            string Name { get; }
        }

        class GenericThing<T> : IThing<T>
        {
            public GenericThing() : this(null, null)
            { }

            public GenericThing(Provider provider) : this(provider, null)
            { }

            public GenericThing(Provider provider, string _)
            {
                Name = typeof(T).Name;
                Provider = provider;
            }

            public string Name { get; }

            public Provider Provider { get; }

        }

        [Fact]
        void TestWithOpenGeneric()
        {
            var services = new ServiceCollection();
            services.AddSingleton(typeof(Provider));
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
