namespace Examples.Management.Tests;

/// <summary>
/// Tests <see cref="DisposableObject" /> class.
/// </summary>
public partial class DisposableObjectTests
{

    [Fact]
    public void WhenCallingDispose_WithUsingBlock_DisposeTrueIsCalled()
    {
        var mock = new Mock<IVerifier>();
        mock.Setup(x => x.Called("DisposeManagedState"));
        mock.Setup(x => x.Called("FreeUnmanagedResources(True)"));
        mock.Setup(x => x.Called("FreeUnmanagedResources(False)"));
        mock.Setup(x => x.Called("DisposeAsync"));

        // use scope
        using (var disposable = new DisposableTester(mock.Object))
        {
        } // call Dispose()

        mock.Verify(x => x.Called("DisposeManagedState"), Times.Once());
        mock.Verify(x => x.Called("FreeUnmanagedResources(True)"), Times.Once());
        mock.Verify(x => x.Called("FreeUnmanagedResources(False)"), Times.Never());
        mock.Verify(x => x.Called("DisposeAsync"), Times.Never());
        mock.VerifyNoOtherCalls();

        return;
    }


    [Fact]
    public async Task WhenCallingAsyncDispose_WithAwaitUsingBlock_DisposeAsyncIsCalled()
    {
        var mock = new Mock<IVerifier>();
        mock.Setup(x => x.Called("DisposeManagedState"));
        mock.Setup(x => x.Called("FreeUnmanagedResources(True)"));
        mock.Setup(x => x.Called("FreeUnmanagedResources(False)"));
        mock.Setup(x => x.Called("DisposeAsync"));

        // use scope
        await using (var disposable = new DisposableTester(mock.Object))
        {
        } // call DisposeAsync().

        mock.Verify(x => x.Called("DisposeManagedState"), Times.Never());
        mock.Verify(x => x.Called("FreeUnmanagedResources(True)"), Times.Never());
        mock.Verify(x => x.Called("FreeUnmanagedResources(False)"), Times.Once());
        mock.Verify(x => x.Called("DisposeAsync"), Times.Once());
        mock.VerifyNoOtherCalls();

        return;
    }


    [Fact]
    public void WhenNotExplicitlyCalled_DisposeFalseIsCalled()
    {
        var mock = new Mock<IVerifier>();
        mock.Setup(x => x.Called("DisposeManagedState"));
        mock.Setup(x => x.Called("FreeUnmanagedResources(True)"));
        mock.Setup(x => x.Called("FreeUnmanagedResources(False)"));
        mock.Setup(x => x.Called("DisposeAsync"));

        void _doScopedAction()
        {
            var obj = new DisposableTester(mock.Object);
            obj = null;
        }
        _doScopedAction();

        GC.Collect(0, GCCollectionMode.Forced);   // call Dispose(False) only.
        GC.WaitForPendingFinalizers();

        mock.Verify(x => x.Called("DisposeManagedState"), Times.Never());
        mock.Verify(x => x.Called("FreeUnmanagedResources(True)"), Times.Never());
        mock.Verify(x => x.Called("FreeUnmanagedResources(False)"), Times.Once());
        mock.Verify(x => x.Called("DisposeAsync"), Times.Never());
    }
}
