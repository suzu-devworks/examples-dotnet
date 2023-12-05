using System.Reflection;
using System.Runtime.Loader;

namespace Examples.Runtime.Loading;

public class SimpleAssemblyLoadContext : AssemblyLoadContext
{
    public SimpleAssemblyLoadContext() : base(isCollectible: true)
    {
    }

    protected override Assembly? Load(AssemblyName name) => null;

}
