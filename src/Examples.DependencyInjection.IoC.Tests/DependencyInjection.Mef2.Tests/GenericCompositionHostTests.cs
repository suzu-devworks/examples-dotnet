using System.Composition.Convention;
using System.Composition.Hosting;
using System.Reflection;
using Examples.DependencyInjection.IoC.Tests;

namespace Examples.DependencyInjection.Mef2.Tests;

/// <summary>
/// Tests to study dependency injection using generic classes.
/// </summary>
public partial class GenericCompositionHostTests
{

    [Fact]
    public void WhenOpenGenericInjection()
    {
        var mock = new Mock<IVerifier>();
        mock.Setup(x => x.Called("Get Int32."));
        mock.Setup(x => x.Called("Name of Name of Int32,IEnumerable`1."));
        mock.Setup(x => x.Called("Value is 0."));

        mock.Setup(x => x.Called("Get String."));
        mock.Setup(x => x.Called("Name of String,IEnumerable`1."));
        mock.Setup(x => x.Called("Value is ."));

        mock.Setup(x => x.Called("Get DateTime."));
        mock.Setup(x => x.Called("Name of DateTime,IEnumerable`1."));
        mock.Setup(x => x.Called("Value is 0001/01/01 0:00:00."));

        mock.Setup(x => x.Called("Get EventArgs."));
        mock.Setup(x => x.Called("Name of EventArgs,IEnumerable`1."));
        mock.Setup(x => x.Called("Value is ."));

        var conventions = new ConventionBuilder();

        // register open generic types.
        conventions.ForTypesMatching(type =>
                type.GetInterfaces().Any(i =>
                    i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IProvider<>)))
            .Export(builder => builder.AsContractType(typeof(IProvider<>)))
            .SelectConstructor(ctors => ctors.Where(c => c.GetParameters().Length == 2).First())
            .Shared();

        conventions.ForTypesMatching(type =>
                type.GetInterfaces().Any(i =>
                    i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IThing<,>)))
            .Export(builder => builder.AsContractType(typeof(IThing<,>)))
            .SelectConstructor(ctors => ctors.Where(c => c.GetParameters().Length == 1).First())
            .Shared();

        // Create ContainerConfiguration from assembly.
        var configuration = new ContainerConfiguration()
            .WithAssembly(Assembly.GetExecutingAssembly(), conventions);

        // Append mock instance.
        configuration.WithExport<IVerifier>(mock.Object);

        using var container = configuration.CreateContainer();

        // create primitive provider.
        var intProvider = container.GetExport<IProvider<int>>();
        intProvider!.GetThing().Exec(Array.Empty<int>());

        // create string provider.
        var stringProvider = container.GetExport<IProvider<string>>();
        stringProvider!.GetThing().Exec(Array.Empty<string>());

        // create struct provider.
        var dateTimeProvider = container.GetExport<IProvider<DateTime>>();
        dateTimeProvider!.GetThing().Exec(Array.Empty<DateTime>());

        // create class provider.
        var eventArgsProvider = container.GetExport<IProvider<EventArgs>>();
        eventArgsProvider!.GetThing().Exec(Array.Empty<EventArgs>());

        mock.Verify(x => x.Called(It.IsAny<string>()), Times.Exactly(12));
        mock.VerifyNoOtherCalls();
        return;
    }

}
