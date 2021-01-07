using System.Reflection;
using System.Runtime.Loader;

namespace Examples.Unloadability
{
    public class CollectibleAssemblyDependencyResolverContext : AssemblyLoadContext
    {
        private readonly AssemblyDependencyResolver Resolver;

        public CollectibleAssemblyDependencyResolverContext(string mainAssemblyToLoadPath)
            : base(isCollectible: true)
        {
            Resolver = new AssemblyDependencyResolver(mainAssemblyToLoadPath);
        }

        protected override Assembly Load(AssemblyName name)
        {
            var assemblyPath = Resolver.ResolveAssemblyToPath(name);
            if (assemblyPath != null)
            {
                return LoadFromAssemblyPath(assemblyPath);
            }

            return null;
        }
    }
}
