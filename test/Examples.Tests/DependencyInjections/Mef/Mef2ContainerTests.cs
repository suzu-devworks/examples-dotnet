using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.ComponentModel.Composition.Registration;
using System.IO;
using System.Linq;
using Xunit;

#pragma warning disable IDE0051

namespace Examples.DependencyInjections.Mef
{
    public class Mef2ContainerTests
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
            var builder = new RegistrationBuilder();
            builder
                .ForTypesDerivedFrom<IMessagePrinter>()
                .ExportInterfaces()
                .SetCreationPolicy(CreationPolicy.Shared);
            builder
                .ForTypesDerivedFrom<IMessageGenerator>()
                .ExportInterfaces()
                .SetCreationPolicy(CreationPolicy.Shared);

            builder
                .ForTypesDerivedFrom<IMyService>()
                .ExportInterfaces()
                .SetCreationPolicy(CreationPolicy.NonShared);

            var catalogs = new AggregateCatalog();
            var assemblyCatalog = new AssemblyCatalog(typeof(IMyService).Assembly, builder);
            catalogs.Catalogs.Add(assemblyCatalog);

            var pruginDir = new DirectoryInfo("plugins");
            if (!pruginDir.Exists)
            {
                pruginDir.Create();
            }
            var dirCatalog = new DirectoryCatalog(pruginDir.Name);
            catalogs.Catalogs.Add(dirCatalog);

            var container = new CompositionContainer(catalogs);

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

