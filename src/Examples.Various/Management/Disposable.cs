namespace Examples.Management;

public static class Disposable
{
    public static void DisposeAny(object o)
    {
        DisposeAnyAsync(o).AsTask().GetAwaiter().GetResult();
    }

    public static async ValueTask DisposeAnyAsync(object o)
    {
        switch (o)
        {
            case IAsyncDisposable asyncDisposable:
                await asyncDisposable.DisposeAsync().ConfigureAwait(false);
                break;
            case IDisposable disposable:
                disposable.Dispose();
                break;
            default:
                break;
        }
    }

}
