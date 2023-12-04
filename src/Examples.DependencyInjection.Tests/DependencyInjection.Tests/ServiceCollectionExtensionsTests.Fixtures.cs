namespace Examples.DependencyInjection.Tests;

#pragma warning disable IDE0040

partial class ServiceCollectionExtensionsTests
{

    public interface IDependency
    {
    }

    [SingletonServiceRegistration(ServiceType = typeof(IDependency))]
    [SingletonServiceRegistration]
    public class DependsOnSingleton : IDependency
    {
    }

    [ScopedServiceRegistration(ServiceType = typeof(IDependency))]
    [ScopedServiceRegistration]
    public class DependsOnScoped : IDependency
    {
    }

    [TransientServiceRegistration(ServiceType = typeof(IDependency))]
    [TransientServiceRegistration]
    public class DependsOnTransient : IDependency
    {
    }

}
