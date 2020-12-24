using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.IO;
using System.Linq;
using Xunit;

#pragma warning disable IDE0051

namespace Examples.DependencyInjections.Mef
{
    public class Mef1ContainerTests
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

        [Export(typeof(IMessagePrinter))]
        [PartCreationPolicy(CreationPolicy.Shared)]
        class ConsoleMessagePrinter : IMessagePrinter
        {
            public void Print(string message) => Console.WriteLine(message);
        }

        [Export(typeof(IMessageGenerator))]
        [PartCreationPolicy(CreationPolicy.Shared)]
        class MyMessageGenerator : IMessageGenerator
        {
            public string Generate() => "Hello MEF Light world.";
        }

        [Export(typeof(IMessageGenerator))]
        [PartCreationPolicy(CreationPolicy.Shared)]
        class MyMessageGenerator2 : IMessageGenerator
        {
            public string Generate() => "How to use MEF light.";
        }

        [Export(typeof(IMyService))]
        [PartCreationPolicy(CreationPolicy.NonShared)]
        class MyService : IMyService
        {
            //[Import]
            private readonly IMessagePrinter _messagePrinter;
            //[ImportMany]
            private readonly IEnumerable<IMessageGenerator> _messageGenerators;

            public MyService()
            {
                //do not called.
            }

            [ImportingConstructor]
            public MyService(IMessagePrinter messagePrinter,
                [ImportMany] IEnumerable<IMessageGenerator> messageGenerators)
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
            var catalogs = new AggregateCatalog();
            var assemblyCatalog = new AssemblyCatalog(typeof(IMyService).Assembly);
            catalogs.Catalogs.Add(assemblyCatalog);

            var pruginDir = new DirectoryInfo("plugins");
            if (!pruginDir.Exists)
            {
                pruginDir.Create();
            }
            var dirCatalog = new DirectoryCatalog(pruginDir.Name);
            catalogs.Catalogs.Add(dirCatalog);

            using var container = new CompositionContainer(catalogs);

            container.ComposeParts(this);

            var service = container.GetExportedValue<IMyService>();
            var other = container.GetExportedValue<IMyService>();
            //Console.WriteLine($"service = [{service.GetHashCode(),10}]");
            //Console.WriteLine($"other   = [{other.GetHashCode(),10}]");
            Assert.NotSame(service, other);

            var generators = container.GetExportedValues<IMessageGenerator>();
            var otherGenerators = container.GetExportedValues<IMessageGenerator>();

            foreach (var (g, o) in Enumerable.Zip(generators, otherGenerators))
            {
                Assert.Same(g, o);
            }

            return;
        }
    }
}

