using System;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using Xunit;

#pragma warning disable IDE0051

namespace Examples.DI.Core
{
    public class CoreContainerTests
    {
        class ConsoleMessagePrinter : IMessagePrinter
        {
            public void Print(string message) => Console.WriteLine(message);
        }

        class MyMessageGenerator : IMessageGenerator
        {
            public string Generate() => "Hello DI world.";
        }

        class MyMessageGenerator2 : IMessageGenerator
        {
            public string Generate() => "Hello DI world too.";
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
        void TestWithSimple()
        {
            var services = new ServiceCollection();
            services.AddTransient<IMyService, MyService>();
            services.AddTransient<IMessagePrinter, ConsoleMessagePrinter>();
            services.AddTransient<IMessageGenerator, MyMessageGenerator>();

            using var provider = services.BuildServiceProvider();

            var service = provider.GetService<IMyService>();
            service.Greet();

            var other = provider.GetService<IMyService>();
            Console.WriteLine($"service = [{service.GetHashCode(),10}]");
            Console.WriteLine($"other   = [{other.GetHashCode(),10}]");
            Assert.NotSame(service, other);

            return;
        }

        [Fact]
        void TestWithScope()
        {
            var services = new ServiceCollection();
            services.AddScoped<IMyService, MyService>();
            services.AddSingleton<IMessagePrinter, ConsoleMessagePrinter>();
            services.AddSingleton<IMessageGenerator, MyMessageGenerator>();

            using var provider = services.BuildServiceProvider();

            IMyService service1;
            Console.WriteLine("Scope1");
            using (var scope = provider.CreateScope())
            {
                var service = scope.ServiceProvider.GetService<IMyService>();
                service.Greet();

                var other = scope.ServiceProvider.GetService<IMyService>();
                Console.WriteLine($"service = [{service.GetHashCode(),10}]");
                Console.WriteLine($"other   = [{other.GetHashCode(),10}]");
                Assert.Same(service, other);
                service1 = service;
            }

            IMyService service2;
            Console.WriteLine("Scope2");
            using (var scope = provider.CreateScope())
            {
                var service = scope.ServiceProvider.GetService<IMyService>();
                service.Greet();

                var other = scope.ServiceProvider.GetService<IMyService>();
                Console.WriteLine($"service = [{service.GetHashCode(),10}]");
                Console.WriteLine($"other   = [{other.GetHashCode(),10}]");
                Assert.Same(service, other);
                service2 = service;
            }

            Assert.NotSame(service1, service2);
            return;
        }

        [Fact]
        void TestWithProvider()
        {
            var services = new ServiceCollection();
            services.AddScoped<IMyService, MyService>(provider =>
            {
                var printer = provider.GetRequiredService<IMessagePrinter>();
                var generator = new MyMessageGenerator2();
                return new MyService(printer, new[] { generator });
            });
            services.AddSingleton<IMessagePrinter, ConsoleMessagePrinter>();
            services.AddSingleton<IMessageGenerator, MyMessageGenerator>();

            using var provider = services.BuildServiceProvider();

            var myService = provider.GetService<IMyService>();
            myService.Greet();

            return;
        }

        [Fact]
        void TestWithEnumable()
        {
            var services = new ServiceCollection();
            services.AddScoped<IMyService, MyService>();
            services.AddSingleton<IMessagePrinter, ConsoleMessagePrinter>();
            services.AddSingleton<IMessageGenerator, MyMessageGenerator>();
            services.AddSingleton<IMessageGenerator, MyMessageGenerator2>();

            using var provider = services.BuildServiceProvider();

            var myService = provider.GetService<IMyService>();
            myService.Greet();

            return;
        }

    }

}
