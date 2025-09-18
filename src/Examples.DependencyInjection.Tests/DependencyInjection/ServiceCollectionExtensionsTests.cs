using Examples.DependencyInjection;
using Examples.Tests.DependencyInjection.Fixtures.AutoDependency;
using Microsoft.Extensions.DependencyInjection;

namespace Examples.Tests.DependencyInjection;

public class ServiceCollectionExtensionsTests
{
    [Fact]
    public void AddServiceWithAttributes_WithAnyServices_CanGetAutoRegisteredServices()
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

    [Fact]
    public void AddServiceWithAttributes_WithAnyServices_CanGetWithinScopeSpecifiedAtRegistration()
    {
        var services = new ServiceCollection();

        var provider = services
            .AddServiceWithAttributes()
            .BuildServiceProvider();

        var singleton = provider.GetService<DependsOnSingleton>();
        var scoped = provider.GetService<DependsOnScoped>();
        var transient = provider.GetService<DependsOnTransient>();

        using (var scope = provider.CreateScope())
        {
            // singleton 
            var singletonInScope = scope.ServiceProvider.GetService<DependsOnSingleton>();
            singletonInScope.IsSameReferenceAs(singleton);

            // scoped
            var scopedInScope1 = scope.ServiceProvider.GetService<DependsOnScoped>();
            var scopedInScope2 = scope.ServiceProvider.GetService<DependsOnScoped>();
            scopedInScope1.IsNotSameReferenceAs(scoped);
            scopedInScope2.IsNotSameReferenceAs(scoped);
            scopedInScope2.IsSameReferenceAs(scopedInScope2);

            // transient
            var transientInScope1 = scope.ServiceProvider.GetService<DependsOnTransient>();
            var transientInScope2 = scope.ServiceProvider.GetService<DependsOnTransient>();
            transientInScope1.IsNotSameReferenceAs(transient);
            transientInScope2.IsNotSameReferenceAs(transient);
            transientInScope1.IsNotSameReferenceAs(transientInScope2);
        }
    }


}
