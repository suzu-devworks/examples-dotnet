using Examples.DependencyInjection;

namespace Examples.Tests.DependencyInjection;

#pragma warning disable IDE0040

partial class ServiceCollectionExtensionsTests
{

    public interface IDependency
    {
    }

    [SingletonServiceRegistoartion(ServiceType = typeof(IDependency))]
    [SingletonServiceRegistoartion]
    public class DependsOnSingleton : IDependency
    {
    }

    [ScopedServiceRegistoartion(ServiceType = typeof(IDependency))]
    [ScopedServiceRegistoartion]
    public class DependsOnScoped : IDependency
    {
    }

    [TransientServiceRegistoartion(ServiceType = typeof(IDependency))]
    [TransientServiceRegistoartion]
    public class DependsOnTransient : IDependency
    {
    }

}
