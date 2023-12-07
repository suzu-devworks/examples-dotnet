namespace Examples.Management;

/// <summary>
/// Provides a base class for implementing the dispose pattern to free unmanaged resources.
/// </summary>
/// <seealso href="https://docs.microsoft.com/ja-jp/dotnet/standard/garbage-collection/implementing-dispose" />
/// <seealso href="https://docs.microsoft.com/ja-jp/dotnet/standard/garbage-collection/implementing-disposeasync" />
public abstract class DisposableObject : IDisposable, IAsyncDisposable
{
    private bool _disposed = false;

    ~DisposableObject() => Dispose(disposing: false);

    public void Dispose()
    {
        Dispose(disposing: true);
        GC.SuppressFinalize(this);
        return;
    }

    public async ValueTask DisposeAsync()
    {
        // Platform async cleanup.
        await DisposeAsyncCore().ConfigureAwait(continueOnCapturedContext: false);

        // Dispose of unmanaged resources.
        Dispose(disposing: false);

        GC.SuppressFinalize(this);

        return;
    }

    protected virtual ValueTask DisposeAsyncCore()
    {
        return ValueTask.CompletedTask;
    }

    protected void Dispose(bool disposing)
    {
        if (_disposed)
        {
            return;
        }
        _disposed = true;

        if (disposing)
        {
            DisposeManagedState();
        }

        FreeUnmanagedResources(disposing);

        // TODO: set large fields to null

        return;
    }

    protected virtual void DisposeManagedState()
    {
        // dispose managed state (managed objects)
    }

    protected virtual void FreeUnmanagedResources(bool disposing)
    {
        // free unmanaged resources (unmanaged objects) and override finalizer
    }


}
