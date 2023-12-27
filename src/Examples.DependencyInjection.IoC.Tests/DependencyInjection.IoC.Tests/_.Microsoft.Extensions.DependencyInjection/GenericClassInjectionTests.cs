using Microsoft.Extensions.DependencyInjection;

namespace Examples.DependencyInjection.IoC.Tests._.Microsoft.Extensions.DependencyInjection;

/// <summary>
/// Tests to study dependency injection using generic classes.
/// </summary>
/// <seealso href="https://www.stevejgordon.co.uk/asp-net-core-dependency-injection-how-to-register-generic-types" />
public partial class GenericClassInjectionTests
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

        var services = new ServiceCollection();

        // register open generic types.
        services.AddSingleton<IVerifier>(_ => mock.Object);
        services.AddSingleton(typeof(IThing<,>), typeof(GenericThing<,>));
        services.AddSingleton(typeof(IProvider<>), typeof(GenericProvider<>));

        using var provider = services.BuildServiceProvider();

        // create primitive provider.
        var intProvider = provider.GetService<IProvider<int>>();
        intProvider!.GetThing().Exec(Array.Empty<int>());

        // create string provider.
        var stringProvider = provider.GetService<IProvider<string>>();
        stringProvider!.GetThing().Exec(Array.Empty<string>());

        // create struct provider.
        var dateTimeProvider = provider.GetService<IProvider<DateTime>>();
        dateTimeProvider!.GetThing().Exec(Array.Empty<DateTime>());

        // create class provider.
        var eventArgsProvider = provider.GetService<IProvider<EventArgs>>();
        eventArgsProvider!.GetThing().Exec(Array.Empty<EventArgs>());

        mock.Verify(x => x.Called(It.IsAny<string>()), Times.Exactly(12));
        mock.VerifyNoOtherCalls();
        return;
    }

}
