using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.Loader;
using Examples.Metaprogramming.Extensions;

namespace Examples.Plugins.Tests.Runtime.Loader;

[Collection(TestCollectionNames.UseGC)]
public class ResolverAssemblyLoadContextTests
{
    private static ITestOutputHelper? Output => TestContext.Current.TestOutputHelper;

    public class ResolverAssemblyLoadContext(string mainAssemblyToLoadPath) : AssemblyLoadContext(isCollectible: true)
    {
        private AssemblyDependencyResolver _resolver = new(mainAssemblyToLoadPath);

        protected override Assembly? Load(AssemblyName name)
        {
            string? assemblyPath = _resolver.ResolveAssemblyToPath(name);
            if (assemblyPath is not null)
            {
                return LoadFromAssemblyPath(assemblyPath);
            }

            return null;
        }

        protected override IntPtr LoadUnmanagedDll(string unmanagedDllName)
        {
            string? libraryPath = _resolver.ResolveUnmanagedDllToPath(unmanagedDllName);
            if (libraryPath is not null)
            {
                return LoadUnmanagedDllFromPath(libraryPath);
            }

            return IntPtr.Zero;
        }
    }

    [Fact]
    public void When_PluginLoaded_WithNoDependencies_Then_CanBeExecuted()
    {
        // Given
        var assemblyPath = Path.GetFullPath(
            @"./plugins/Examples.Plugins.Hello/Examples.Plugins.Hello.dll");
        var alc = new ResolverAssemblyLoadContext(assemblyPath);

        // When
        var assembly = alc.LoadFromAssemblyPath(assemblyPath);
        using TextWriter writer = new StringWriter();

        assembly.GetTypesImplementingInterface<Plugins.Tutorials.ICommand>()
            .ToList().ForEach(t =>
            {
                var instance = Activator.CreateInstance(t);
                t.GetMethod("Execute")?.Invoke(instance, [writer]);
            });

        // Then
        Assert.NotNull(assembly);
    }

    [Fact]
    public void When_PluginLoaded_WithDependencies_Then_CanBeExecuted()
    {
        // Given
        var assemblyPath = Path.GetFullPath(
            @"./plugins/Examples.Plugins.Json/Examples.Plugins.Json.dll");
        var alc = new ResolverAssemblyLoadContext(assemblyPath);

        // When
        var assembly = alc.LoadFromAssemblyPath(assemblyPath);
        using TextWriter writer = new StringWriter();

        assembly.GetTypesImplementingInterface<Plugins.Tutorials.ICommand>()
            .ToList().ForEach(t =>
            {
                var instance = Activator.CreateInstance(t); // This line does not cause an error.
                t.GetMethod("Execute")?.Invoke(instance, [writer]); // This line is causing an error!
            });

        // Then
        Assert.NotNull(assembly);
    }

    [Fact]
    public void When_PluginLoaded_WithLibraryDependencies_Then_CanBeExecuted()
    {
        // Given
        var assemblyPath = Path.GetFullPath(
            @"./plugins/Examples.Plugins.Libuv/Examples.Plugins.Libuv.dll");
        var alc = new ResolverAssemblyLoadContext(assemblyPath);

        // When
        var assembly = alc.LoadFromAssemblyPath(assemblyPath);
        using TextWriter writer = new StringWriter();

        assembly.GetTypesImplementingInterface<Plugins.Tutorials.ICommand>()
            .ToList().ForEach(t =>
            {
                var instance = Activator.CreateInstance(t); // This line does not cause an error.
                t.GetMethod("Execute")?.Invoke(instance, [writer]); // This line is causing an error!
            });

        // Then
        Assert.NotNull(assembly);
    }

    [Fact]
    public void When_UnloadsAssembly_Then_WeakReferenceBecomesInvalid()
    {
        [MethodImpl(MethodImplOptions.NoInlining)]
        static void ExecuteAndUnload(string assemblyPath, out WeakReference alcWeakRef)
        {
            var alc = new ResolverAssemblyLoadContext(assemblyPath);
            var assembly = alc.LoadFromAssemblyPath(assemblyPath);

            alcWeakRef = new WeakReference(alc, trackResurrection: true);

            alc.Unloading += context =>
            {
                Output?.WriteLine($"Unloading {context.Name}");
            };

            var types = assembly.GetTypesImplementingInterface<Plugins.Tutorials.ICommand>();
            Assert.NotEmpty(types);

            alc.Unload();
        }

        // Given
        var assemblyPath = Path.GetFullPath(
            @"./plugins/Examples.Plugins.Hello/Examples.Plugins.Hello.dll");

        // When
        ExecuteAndUnload(assemblyPath, out var testAlcWeakRef);

        for (int i = 0; testAlcWeakRef.IsAlive && (i < 10); i++)
        {
            GC.Collect();
            GC.WaitForPendingFinalizers();
        }

        // Assert
        Assert.False(testAlcWeakRef.IsAlive,
            "The AssemblyLoadContext should have been collected after disposal.");
    }
}
