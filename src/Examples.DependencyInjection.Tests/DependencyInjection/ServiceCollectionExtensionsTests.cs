
using Examples.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;

namespace Examples.Tests.DependencyInjection;

public partial class ServiceCollectionExtensionsTests
{
    [Fact]
    public void WhenCallingAddServiceWithAttributes_Success()
    {
        var services = new ServiceCollection();

        var provider = services
                        .AddServiceWithAttributes()
                        .BuildServiceProvider();

        provider.GetService<DependsOnSingleton>()!.IsInstanceOf<DependsOnSingleton>();
        provider.GetService<DependsOnScoped>()!.IsInstanceOf<DependsOnScoped>();
        provider.GetService<DependsOnTransient>()!.IsInstanceOf<DependsOnTransient>();

        provider.GetServices<IDependency>().Count().Is(3);
    }

}
