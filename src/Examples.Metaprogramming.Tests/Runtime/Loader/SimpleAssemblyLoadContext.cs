using System.Reflection;
using System.Runtime.Loader;

namespace Examples.Metaprogramming.Runtime.Loader;

public class SimpleAssemblyLoadContext : AssemblyLoadContext
{
    public SimpleAssemblyLoadContext() : base(isCollectible: true)
    {
    }

    protected override Assembly? Load(AssemblyName name) => null;

}
