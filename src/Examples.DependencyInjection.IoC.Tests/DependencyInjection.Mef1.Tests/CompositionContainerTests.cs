using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.Reflection;

namespace Examples.DependencyInjection.Mef1.Tests;

/// <summary>
/// Tests to study dependency injection using MEF 4.0 (<see cref="System.ComponentModel.Composition" />).
/// </summary>
public partial class CompositionContainerTests
{

    [Fact]
    public void WhenMultipleInjection()
    {
        var mock = new Mock<IMessagePrinter>();
        mock.Setup(x => x.Print("Hello MEF DI world 1st."));
        mock.Setup(x => x.Print("Hello MEF DI world 2nd."));
        mock.Setup(x => x.Print("Hello MEF DI world 3rd."));

        // Create Aggregate.
        var aggregate = new AggregateCatalog();

        // Create AssemblyCatalog.
        var assemblyCatalog = new AssemblyCatalog(Assembly.GetExecutingAssembly());
        aggregate.Catalogs.Add(assemblyCatalog);

        // Create DirectoryCatalog for plugins.
        var pluginDir = new DirectoryInfo("plugins");
        if (!pluginDir.Exists) { pluginDir.Create(); }
        var dirCatalog = new DirectoryCatalog(pluginDir.Name);
        aggregate.Catalogs.Add(dirCatalog);

        using var container = new CompositionContainer(aggregate);
        container.ComposeParts(this);

        // Append mock instance.
        container.ComposeExportedValue<IMessagePrinter>(mock.Object);

        var service = container.GetExportedValue<IMyService>();
        service?.Greet();

        var other = container.GetExportedValue<IMyService>();
        other?.Greet();

        // The instance is also different because CreationPolicy.NonShared is specified.
        object.ReferenceEquals(service, other).IsFalse();

        mock.Verify(x => x.Print("Hello MEF DI world 1st."), Times.Exactly(2));
        mock.Verify(x => x.Print("Hello MEF DI world 2nd."), Times.Exactly(2));
        mock.Verify(x => x.Print("Hello MEF DI world 3rd."), Times.Exactly(2));
        mock.VerifyNoOtherCalls();
        return;
    }

}
