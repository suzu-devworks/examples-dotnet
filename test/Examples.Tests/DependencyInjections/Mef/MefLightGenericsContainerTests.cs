using System;
using System.Composition.Convention;
using System.Composition.Hosting;
using System.Linq;
using Xunit;

#pragma warning disable IDE0051

namespace Examples.DependencyInjections.Core
{
    public class MefLightGenericsContainerTests
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
            var conventions = new ConventionBuilder();
            conventions
                .ForType<Provider>()
                .Export()
                .Shared();
            conventions
                .ForTypesMatching(type => type.IsGenericType
                    && type.GetInterfaces()
                        .Any(i => i.GetGenericTypeDefinition() == typeof(IThing<>)))
                .SelectConstructor(ctors => ctors.Where(c => c.GetParameters().Length == 1).First())
                .Export(builder => builder.AsContractType(typeof(IThing<>)))
                .Shared();

            var assemblies = new[] { typeof(IThing<>).Assembly };

            var configuration = new ContainerConfiguration()
                .WithAssemblies(assemblies, conventions);

            using var container = configuration.CreateContainer();

            var dateTimeService1 = container.GetExport<IThing<DateTime>>();
            var dateTimeService2 = container.GetExport<IThing<DateTime>>();

            Assert.Same(dateTimeService1, dateTimeService2);

            var intService1 = container.GetExport<IThing<int>>();

            Assert.NotSame(dateTimeService1, intService1);
            Assert.NotSame(dateTimeService2, intService1);

            return;
        }

    }
}
