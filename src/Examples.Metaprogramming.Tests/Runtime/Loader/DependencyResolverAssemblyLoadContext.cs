using System.Reflection;
using System.Runtime.Loader;

namespace Examples.Metaprogramming.Runtime.Loader;

public class DependencyResolverAssemblyLoadContext : AssemblyLoadContext
{
    private readonly AssemblyDependencyResolver _resolver;

    public DependencyResolverAssemblyLoadContext(string mainAssemblyToLoadPath)
        : base(isCollectible: true)
    {
        _resolver = new AssemblyDependencyResolver(mainAssemblyToLoadPath);
    }

    protected override Assembly? Load(AssemblyName name)
    {
        var assemblyPath = _resolver.ResolveAssemblyToPath(name);
        return assemblyPath is null ? null : LoadFromAssemblyPath(assemblyPath);
    }
}
