using System.Composition.Convention;
using System.Composition.Hosting;
using System.Reflection;
using Examples.DependencyInjection.IoC.Tests;

namespace Examples.DependencyInjection.Mef2.Tests;

/// <summary>
/// Tests to study dependency injection using MEF 2 (<see cref="System.Composition" />).
/// </summary>
public partial class CompositionHostTests
{

    [Fact]
    public void WhenMultipleInjection()
    {
        var mock = new Mock<IMessagePrinter>();
        mock.Setup(x => x.Print("Hello MEF'' DI world 1st."));
        mock.Setup(x => x.Print("Hello MEF'' DI world 2nd."));
        mock.Setup(x => x.Print("Hello MEF'' DI world 3rd."));

        // Convention-Driven "lightweight " configuration.
        var conventions = new ConventionBuilder();
        conventions
            .ForTypesDerivedFrom<IMessagePrinter>()
            .ExportInterfaces()
            .Shared();

        // conventions
        //     .ForTypesDerivedFrom<IMessageGenerator>()
        //     .ExportInterfaces()
        //     .Shared();
        conventions
            .ForType<MyMessageGenerator1>()
            .Export<IMessageGenerator>()
            .Shared();
        conventions
            .ForType<MyMessageGenerator2>()
            .Export<IMessageGenerator>()
            .Shared();
        conventions
            .ForType<MyMessageGenerator3>()
            .Export<IMessageGenerator>()
            .Shared();

        // conventions
        //     .ForTypesDerivedFrom<IMyService>()
        //     .ExportInterfaces();
        conventions
            .ForType<MyService>()
            .Export<IMyService>();

        // Create ContainerConfiguration from assembly.
        var configuration = new ContainerConfiguration()
            .WithAssembly(Assembly.GetExecutingAssembly(), conventions)
            .WithExport<IMessagePrinter>(mock.Object);

        // Create container.
        using var container = configuration.CreateContainer();

        var service = container.GetExport<IMyService>();
        service!.Greet();

        var other = container.GetExport<IMyService>();
        other!.Greet();

        // The instance is also different because CreationPolicy.NonShared is specified.
        object.ReferenceEquals(service, other).IsFalse();

        mock.Verify(x => x.Print("Hello MEF'' DI world 1st."), Times.Exactly(2));
        mock.Verify(x => x.Print("Hello MEF'' DI world 2nd."), Times.Exactly(2));
        mock.Verify(x => x.Print("Hello MEF'' DI world 3rd."), Times.Exactly(2));
        mock.VerifyNoOtherCalls();
        return;
    }

}

