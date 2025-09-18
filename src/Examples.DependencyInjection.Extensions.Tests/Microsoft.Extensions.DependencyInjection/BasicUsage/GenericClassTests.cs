using Examples.Tests.Microsoft.Extensions.DependencyInjection.Fixtures.ThingProviding;
using Microsoft.Extensions.DependencyInjection;

namespace Examples.Tests.Microsoft.Extensions.DependencyInjection.BasicUsage;

/// <summary>
/// How to Register Generic Types.
/// </summary>
/// <seealso href="https://www.stevejgordon.co.uk/asp-net-core-dependency-injection-how-to-register-generic-types" />
public class GenericClassTests
{
    [Fact]
    public void WhenRegisteringOpenGenericType_CanGetsClosedConstructedTypes()
    {
        var mock = new Mock<IVerifier>();
        mock.Setup(x => x.Called("Name of Int32,IEnumerable`1."));
        mock.Setup(x => x.Called("Value is 0."));

        mock.Setup(x => x.Called("Name of String,IEnumerable`1."));
        mock.Setup(x => x.Called("Value is ."));

        mock.Setup(x => x.Called("Name of DateTime,IEnumerable`1."));
        mock.Setup(x => x.Called("Value is 0001/01/01 0:00:00."));

        mock.Setup(x => x.Called("Name of EventArgs,IEnumerable`1."));
        mock.Setup(x => x.Called("Value is ."));

        var services = new ServiceCollection();

        // Register open generic types.
        services.AddSingleton<IVerifier>(_ => mock.Object);
        services.AddSingleton(typeof(IThing<,>), typeof(GenericThing<,>));
        services.AddSingleton(typeof(IProvider<>), typeof(GenericProvider<>));

        using var provider = services.BuildServiceProvider();

        // Gets primitive provider.
        var intProvider = provider.GetService<IProvider<int>>();
        intProvider!.GetThing().Exec(Enumerable.Empty<int>());

        // Gets string provider.
        var stringProvider = provider.GetService<IProvider<string>>();
        stringProvider!.GetThing().Exec(Enumerable.Empty<string>());

        // Gets struct provider.
        var dateTimeProvider = provider.GetService<IProvider<DateTime>>();
        dateTimeProvider!.GetThing().Exec(Enumerable.Empty<DateTime>());

        // Gets class provider.
        var eventArgsProvider = provider.GetService<IProvider<EventArgs>>();
        eventArgsProvider!.GetThing().Exec(Enumerable.Empty<EventArgs>());

        mock.Verify(x => x.Called(It.IsAny<string>()), Times.Exactly(8));
        mock.VerifyNoOtherCalls();
    }

}
