using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.ComponentModel.Composition.Registration;
using System.Reflection;
using Examples.Tests.System.ComponentModel.Composition.Registration.Fixtures.MyServices;

namespace Examples.Tests.System.ComponentModel.Composition.Registration.BasicUsage;

/// <summary>
/// Tests to study dependency injection using MEF 4.5 (<see cref="System.ComponentModel.Composition.Registration" />).
/// </summary>
public partial class CompositionContainerTests
{

    [Fact]
    public void WhenMultipleServicesRegisteredWithSameInterface_ReturnsAsIEnumerable()
    {
        var mock = new Mock<IMessagePrinter>();
        mock.Setup(x => x.Print("Hello MEF' DI world 1st."));
        mock.Setup(x => x.Print("Hello MEF' DI world 2nd."));
        mock.Setup(x => x.Print("Hello MEF' DI world 3rd."));

        // Convention-Driven configuration.
        var builder = new RegistrationBuilder();

        // builder.ForType* ... Define class or a set of classes to be operated on.
        //  .Export* ... Export classes .
        //  ... the other specifies the attributes, metadata and sharing policies to apply to
        //      the selected classes, properties of the classes or constructors of the classes.

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

        // Create Aggregate.
        var aggregate = new AggregateCatalog();

        // Create AssemblyCatalog.
        var assemblyCatalog = new AssemblyCatalog(Assembly.GetExecutingAssembly(), builder);
        aggregate.Catalogs.Add(assemblyCatalog);

        // // Create DirectoryCatalog for plugins.
        // var pluginDir = new DirectoryInfo("plugins");
        // if (!pluginDir.Exists) { pluginDir.Create(); }
        // var dirCatalog = new DirectoryCatalog(pluginDir.Name);
        // aggregate.Catalogs.Add(dirCatalog);

        // Create container.
        using var container = new CompositionContainer(aggregate);
        // container.ComposeParts(this);

        // Append mock instance.
        container.ComposeExportedValue<IMessagePrinter>(mock.Object);

        var service = container.GetExportedValue<IMyService>();
        service?.Greet();

        var other = container.GetExportedValue<IMyService>();
        other?.Greet();

        // The instance is also different because CreationPolicy.NonShared is specified.
        object.ReferenceEquals(service, other).IsFalse();

        mock.Verify(x => x.Print("Hello MEF DI' world 1st."), Times.Exactly(2));
        mock.Verify(x => x.Print("Hello MEF DI' world 2nd."), Times.Exactly(2));
        mock.Verify(x => x.Print("Hello MEF DI' world 3rd."), Times.Exactly(2));
        mock.VerifyNoOtherCalls();
    }

}
