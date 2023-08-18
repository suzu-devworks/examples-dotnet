using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Examples.DependencyInjection;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddServiceWithAttributes(this IServiceCollection servies)
    {
        var provider = servies.BuildServiceProvider();
        var logger = provider.GetService<ILogger<ServiceRegistoartionAttribute>>();

        foreach (var (service, implement) in EnumerateServcieTypes<SingletonServiceRegistoartionAttribute>())
        {
            servies.Add(ServiceDescriptor.Singleton(service, implement));
            logger?.LogDebug(".AddSingleton<{service}, {implement}>", service, implement);
        }

        foreach (var (service, implement) in EnumerateServcieTypes<ScopedServiceRegistoartionAttribute>())
        {
            servies.Add(ServiceDescriptor.Scoped(service, implement));
            logger?.LogDebug(".AddScoped<{service}, {implement}>", service, implement);
        }

        foreach (var (service, implement) in EnumerateServcieTypes<TransientServiceRegistoartionAttribute>())
        {
            servies.Add(ServiceDescriptor.Transient(service, implement));
            logger?.LogDebug(".AddTransient<{service}, {implement}>", service, implement);
        }

        return servies;
    }

    private static IEnumerable<(Type, Type)> EnumerateServcieTypes<TAttribute>()
        where TAttribute : ServiceRegistoartionAttribute
    {
        foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
        {
            var tuples = assembly.GetTypes()
                .SelectMany(t => t.GetCustomAttributes<TAttribute>()
                    .Where(a => a.Enabled)
                    .Select(a => (Attribute: a, ImplementType: t)))
                .Select(x => (x.Attribute.ServiceType ?? x.ImplementType, x.ImplementType));

            foreach (var tuple in tuples)
            {
                yield return tuple;
            }
        }
    }

}
