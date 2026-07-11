using System.Runtime.CompilerServices;
using Examples.Various.Tests;
using NSubstitute;

namespace Examples.Management.Tests;

/// <summary>
/// Tests <see cref="DisposableObject" /> class.
/// </summary>
[Collection(TestCollectionNames.UseGC)]
public partial class DisposableObjectTests
{
    [Fact]
    public void When_UsedWithUsingScope_Then_DisposeIsCalled()
    {
        var verifier = Substitute.For<IVerifier>();

        using (var disposable = new DisposableTester(verifier))
        {
            // any codes.

        } // call Dispose()

        verifier.Received(1).Called("DisposeManagedState");
        verifier.Received(1).Called("FreeUnmanagedResources(True)");
        verifier.DidNotReceive().Called("FreeUnmanagedResources(False)");
        verifier.DidNotReceive().Called("DisposeAsync");
    }

    [Fact]
    public async Task When_UsingWithAwaitUsingScope_Then_DisposeAsyncIsCalled()
    {
        var verifier = Substitute.For<IVerifier>();

        // use scope
        await using (var disposable = new DisposableTester(verifier))
        {
            // any codes.

        } // call DisposeAsync().

        verifier.DidNotReceive().Called("DisposeManagedState");
        verifier.DidNotReceive().Called("FreeUnmanagedResources(True)");
        verifier.Received(1).Called("FreeUnmanagedResources(False)");
        verifier.Received(1).Called("DisposeAsync");
    }

    [Fact]
    public void When_NotExplicitlyCalled_Then_FinalizerIsCalled()
    {
        var verifier = Substitute.For<IVerifier>();

        [MethodImpl(MethodImplOptions.NoInlining)]
        static void DoScopedAction(IVerifier verifier, out WeakReference testAlcWeakRef)
        {
            var obj = new DisposableTester(verifier);
            testAlcWeakRef = new WeakReference(obj);
            obj = null;
            _ = obj; // prevent warning.
        }

        DoScopedAction(verifier, out var testAlcWeakRef);

        for (int i = 0; testAlcWeakRef.IsAlive && (i < 10); i++)
        {
            GC.Collect();
            GC.WaitForPendingFinalizers();
        }

        verifier.DidNotReceive().Called("DisposeManagedState");
        verifier.DidNotReceive().Called("FreeUnmanagedResources(True)");
        verifier.Received(1).Called("FreeUnmanagedResources(False)");
        verifier.DidNotReceive().Called("DisposeAsync");
    }
}
