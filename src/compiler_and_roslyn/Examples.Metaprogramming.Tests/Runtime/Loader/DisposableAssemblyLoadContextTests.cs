using System.Runtime.CompilerServices;
using Examples.Metaprogramming.Runtime.Loader;

namespace Examples.Metaprogramming.Tests.Runtime.Loader;

[Collection(TestCollectionNames.UseGC)]
public class DisposableAssemblyLoadContextTests
{
    [Fact]
    public void Constructor_WithName_CreatesCollectibleContext()
    {
        // Arrange & Act
        using var alc = new DisposableAssemblyLoadContext("TestContext");

        // Assert
        Assert.NotNull(alc);
        Assert.True(alc.IsCollectible);
        Assert.Equal("TestContext", alc.Name);
    }

    [Fact]
    public void Constructor_WithNullName_CreatesCollectibleContext()
    {
        // Arrange & Act
        using var alc = new DisposableAssemblyLoadContext(null);

        // Assert
        Assert.NotNull(alc);
        Assert.True(alc.IsCollectible);
        Assert.Null(alc.Name);
    }

    [Fact]
    public void Dispose_IsIdempotent_CanBeCalledMultipleTimes()
    {
        // Arrange
        var alc = new DisposableAssemblyLoadContext("MultiDisposeContext");
        Assert.True(alc.IsCollectible);

        // Act & Assert
        alc.Dispose(); // Should not throw
        alc.Dispose(); // Should not throw (idempotent)

        Assert.True(true); // Successfully disposed multiple times
    }

    [Fact]
    public void Dispose_UnloadsAssembly_WeakReferenceBecomesInvalid()
    {
        [MethodImpl(MethodImplOptions.NoInlining)]
        static void ExecuteAndUnload(string name, string assemblyPath, out WeakReference alcWeakRef)
        {
            var alc = new DisposableAssemblyLoadContext(name);
            var assembly = alc.LoadFromAssemblyPath(assemblyPath);

            alcWeakRef = new WeakReference(alc, trackResurrection: true);

            var type = assembly.GetType("Xunit.Assert"); // Use the assembly to ensure it's loaded
            Assert.NotNull(type);

            alc.Dispose();
        }

        // Arrange
        var assemblyPath = typeof(Xunit.Assert).Assembly.Location;

        // Act
        ExecuteAndUnload("UnloadTestContext", assemblyPath, out var testAlcWeakRef);

        for (int i = 0; testAlcWeakRef.IsAlive && (i < 10); i++)
        {
            GC.Collect();
            GC.WaitForPendingFinalizers();
        }

        // Assert
        Assert.False(testAlcWeakRef.IsAlive, "The AssemblyLoadContext should have been collected after disposal.");
    }

}
