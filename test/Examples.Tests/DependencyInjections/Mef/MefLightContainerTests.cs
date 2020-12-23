using System;
using System.Collections.Generic;
using System.Composition.Convention;
using System.Composition.Hosting;
using System.Linq;
using Xunit;

#pragma warning disable IDE0051

namespace Examples.DependencyInjections.Core
{
    public class MefLightContainer
    {
        interface IMessagePrinter
        {
            void Print(string message);
        }
        interface IMessageGenerator
        {
            string Generate();
        }

        interface IMyService
        {
            void Greet();
        }

        class ConsoleMessagePrinter : IMessagePrinter
        {
            public void Print(string message) => Console.WriteLine(message);
        }

        class MyMessageGenerator : IMessageGenerator
        {
            public string Generate() => "Hello MEF Light world.";
        }

        class MyMessageGenerator2 : IMessageGenerator
        {
            public string Generate() => "How to use MEF light.";
        }

        class MyService : IMyService
        {
            private readonly IMessagePrinter _messagePrinter;
            private readonly IEnumerable<IMessageGenerator> _messageGenerators;

            public MyService()
            {
                //do not called.
            }

            public MyService(IMessagePrinter messagePrinter,
                IEnumerable<IMessageGenerator> messageGenerators)
            {
                _messagePrinter = messagePrinter;
                _messageGenerators = messageGenerators;
            }

            public void Greet()
            {
                foreach (var greater in _messageGenerators)
                {
                    _messagePrinter.Print(greater.Generate());
                }
            }
        }

        [Fact]
        void TestWithCompositon()
        {
            var conventions = new ConventionBuilder();
            conventions
                .ForTypesDerivedFrom<IMessagePrinter>()
                .ExportInterfaces()
                .Shared();
            conventions
                .ForTypesDerivedFrom<IMessageGenerator>()
                .ExportInterfaces()
                .Shared();

            conventions
                .ForTypesDerivedFrom<IMyService>()
                .ExportInterfaces();

            var assemblies = new[] { typeof(IMyService).Assembly };

            var configuration = new ContainerConfiguration()
                .WithAssemblies(assemblies, conventions);

            using var container = configuration.CreateContainer();

            var service = container.GetExport<IMyService>();
            var other = container.GetExport<IMyService>();
            //Console.WriteLine($"service = [{service.GetHashCode(),10}]");
            //Console.WriteLine($"other   = [{other.GetHashCode(),10}]");
            Assert.NotSame(service, other);

            var generators = container.GetExports<IMessageGenerator>();
            var otherGenerators = container.GetExports<IMessageGenerator>();

            foreach (var (g, o) in Enumerable.Zip(generators, otherGenerators))
            {
                Assert.Same(g, o);
            }

            return;
        }
    }
}

