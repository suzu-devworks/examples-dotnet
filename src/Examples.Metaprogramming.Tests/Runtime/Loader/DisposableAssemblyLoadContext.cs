using System.Runtime.Loader;

namespace Examples.Metaprogramming.Runtime.Loader;

public sealed class DisposableAssemblyLoadContext(string? name)
    : AssemblyLoadContext(name, isCollectible: true), IDisposable
{
    public void Dispose()
    {
        if (IsCollectible)
        {
            Unload();
        }
        GC.SuppressFinalize(this);
    }

}

