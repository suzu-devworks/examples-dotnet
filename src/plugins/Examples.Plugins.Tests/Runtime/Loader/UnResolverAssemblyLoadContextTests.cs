using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.Loader;
using Examples.Metaprogramming.Extensions;

namespace Examples.Plugins.Tests.Runtime.Loader;

[Collection(TestCollectionNames.UseGC)]
public class UnResolverAssemblyLoadContextTests
{
    private static ITestOutputHelper? Output => TestContext.Current.TestOutputHelper;

    public class UnResolverAssemblyLoadContext() : AssemblyLoadContext(isCollectible: true)
    {
        // It is stated that returning null causes all dependent assemblies
        // to be loaded into the default context.
        protected override Assembly? Load(AssemblyName name) => null;
    }

    [Fact]
    public void When_PluginLoaded_WithNoDependencies_Then_CanBeExecuted()
    {
        // Given
        var assemblyPath = Path.GetFullPath(
            @"./plugins/Examples.Plugins.Hello/Examples.Plugins.Hello.dll");
        var alc = new UnResolverAssemblyLoadContext();

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
    public void When_PluginLoaded_WithDependencies_Then_ThrowsFileNotFoundException()
    {
        // Given
        var assemblyPath = Path.GetFullPath(
            @"./plugins/Examples.Plugins.Json/Examples.Plugins.Json.dll");
        var alc = new UnResolverAssemblyLoadContext();

        // When
        var assembly = alc.LoadFromAssemblyPath(assemblyPath);
        using TextWriter writer = new StringWriter();

        var exception = Record.Exception(() =>
        {
            assembly.GetTypesImplementingInterface<Plugins.Tutorials.ICommand>()
                .ToList().ForEach(t =>
                {
                    var instance = Activator.CreateInstance(t); // This line does not cause an error.
                    t.GetMethod("Execute")?.Invoke(instance, [writer]); // This line is causing an error!
                });
        });

        // Then
        Assert.NotNull(assembly); // The assembly is loaded successfully

        Assert.IsType<TargetInvocationException>(exception);
        Assert.IsType<FileNotFoundException>(exception?.InnerException);
        Assert.Contains("'Newtonsoft.Json,", exception?.InnerException?.Message);
    }

    [Fact]
    public void When_PluginLoaded_WithLibraryDependencies_Then_ThrowsFileNotFoundException()
    {
        // Given
        var assemblyPath = Path.GetFullPath(
            @"./plugins/Examples.Plugins.Libuv/Examples.Plugins.Libuv.dll");
        var alc = new UnResolverAssemblyLoadContext();

        // When
        var assembly = alc.LoadFromAssemblyPath(assemblyPath);
        using TextWriter writer = new StringWriter();

        var exception = Record.Exception(() =>
        {
            assembly.GetTypesImplementingInterface<Plugins.Tutorials.ICommand>()
                .ToList().ForEach(t =>
                {
                    var instance = Activator.CreateInstance(t); // This line does not cause an error.
                    t.GetMethod("Execute")?.Invoke(instance, [writer]); // This line is causing an error!
                });
        });

        // Then
        Assert.NotNull(assembly); // The assembly is loaded successfully

        Assert.IsType<TargetInvocationException>(exception);
        Assert.IsType<DllNotFoundException>(exception?.InnerException);
        Assert.Contains("'libuv'", exception?.InnerException?.Message);
    }

    [Fact]
    public void When_UnloadsAssembly_Then_WeakReferenceBecomesInvalid()
    {
        [MethodImpl(MethodImplOptions.NoInlining)]
        static void ExecuteAndUnload(string assemblyPath, out WeakReference alcWeakRef)
        {
            var alc = new UnResolverAssemblyLoadContext();
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
