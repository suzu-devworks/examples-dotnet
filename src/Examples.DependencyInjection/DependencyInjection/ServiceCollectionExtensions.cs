using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Examples.DependencyInjection;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddServiceWithAttributes(this IServiceCollection services)
    {
        var provider = services.BuildServiceProvider();
        var logger = provider.GetService<ILogger<ServiceRegistrationAttribute>>();

        foreach (var (service, implement) in EnumerateServiceTypes<SingletonServiceRegistrationAttribute>())
        {
            services.Add(ServiceDescriptor.Singleton(service, implement));
            logger?.LogDebug(".AddSingleton<{service}, {implement}>", service, implement);
        }

        foreach (var (service, implement) in EnumerateServiceTypes<ScopedServiceRegistrationAttribute>())
        {
            services.Add(ServiceDescriptor.Scoped(service, implement));
            logger?.LogDebug(".AddScoped<{service}, {implement}>", service, implement);
        }

        foreach (var (service, implement) in EnumerateServiceTypes<TransientServiceRegistrationAttribute>())
        {
            services.Add(ServiceDescriptor.Transient(service, implement));
            logger?.LogDebug(".AddTransient<{service}, {implement}>", service, implement);
        }

        return services;
    }

    private static IEnumerable<(Type, Type)> EnumerateServiceTypes<TAttribute>()
        where TAttribute : ServiceRegistrationAttribute
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
