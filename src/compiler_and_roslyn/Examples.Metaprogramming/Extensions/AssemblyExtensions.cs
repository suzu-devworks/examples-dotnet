using System.Reflection;

namespace Examples.Metaprogramming.Extensions;

/// <summary>
/// Extension methods for the <see cref="Assembly"/> class.
/// </summary>
public static class AssemblyExtensions
{
    /// <summary>
    /// Returns all types in the assembly that implement the specified interface.
    /// </summary>
    /// <typeparam name="TInterface">The interface type to search for implementations.</typeparam>
    /// <param name="assembly">The assembly to search for types.</param>
    /// <returns>An enumerable collection of types that implement the specified interface.</returns>
    public static IEnumerable<Type> GetTypesImplementingInterface<TInterface>(this Assembly assembly)
    {
        return assembly.GetTypes().Where(t => t.GetInterfaces().Any(x => x == typeof(TInterface)));
    }

    /// <summary>
    /// Returns all types in the assembly that implement the specified interface.
    /// </summary>
    /// <param name="assembly">The assembly to search for types.</param>
    /// <param name="interfaceType">The interface type to search for implementations.</param>
    /// <returns>An enumerable collection of types that implement the specified interface.</returns>
    public static IEnumerable<Type> GetTypesImplementingInterface(this Assembly assembly, Type interfaceType)
    {
        return assembly.GetTypes().Where(t => t.GetInterfaces().Any(x => x == interfaceType));
    }
}
