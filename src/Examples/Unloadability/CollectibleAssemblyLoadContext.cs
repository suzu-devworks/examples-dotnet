using System.Reflection;
using System.Runtime.Loader;

namespace Examples.Unloadability
{
    public class CollectibleAssemblyLoadContext : AssemblyLoadContext
    {
        public CollectibleAssemblyLoadContext() : base(isCollectible: true)
        { }

        protected override Assembly Load(AssemblyName name) => null;
    }
}
