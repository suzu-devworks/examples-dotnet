using Microsoft.Extensions.DependencyInjection;

namespace Examples.DependencyInjection.IoC.Tests._.Microsoft.Extensions.DependencyInjection;

/// <summary>
/// Tests to study dependency injection using <see cref="Microsoft.Extensions.DependencyInjection" />.
/// Multiple Contractor selection.
/// </summary>
public partial class ConstructorSelectionTests
{

    [Fact]
    public void WhenCallingGetService_WithMultipleConstructor()
    {
        var mock = new Mock<IVerifier>();
        mock.Setup(x => x.Called("Hello DI world!!!"));

        var services = new ServiceCollection();
        services.AddSingleton<IVerifier>(_ => mock.Object);
        services.AddSingleton<ClassWithMultipleConstructors>(provider =>
            new ClassWithMultipleConstructors(provider.GetRequiredService<IVerifier>()));

        using var provider = services.BuildServiceProvider();

        // injection IEnumerable<IMessageGenerator>.
        var single = provider.GetService<ClassWithMultipleConstructors>();
        single!.Verify();

        mock.Verify(x => x.Called("Hello DI world!!!"), Times.Once());
        mock.VerifyNoOtherCalls();
        return;
    }


}
