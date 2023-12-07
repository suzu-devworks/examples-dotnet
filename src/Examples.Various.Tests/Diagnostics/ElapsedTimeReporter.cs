using System.Diagnostics;

namespace Examples.Diagnostics;

/// <summary>
/// Elapsed Stopwatch Wrapper used <see cref="IDisposable" />.
/// </summary>
public sealed class ElapsedTimeReporter : IDisposable
{
    private readonly Stopwatch _stopwatch;
    private readonly Action<TimeSpan> _reportAction;
    private bool _disposed;

    private ElapsedTimeReporter(Stopwatch stopwatch, Action<TimeSpan> reportAction)
    {
        _stopwatch = stopwatch;
        _reportAction = reportAction;
        _disposed = false;
    }

    /// <summary>
    /// Gets a value indicating whether the System.Diagnostics.Stopwatch timer is running.
    /// </summary>
    public bool IsRunning => _stopwatch.IsRunning;

    /// <summary>
    /// An Exception caught during Dispose。
    /// </summary>
    /// <value></value>
    public Exception? DisposingException { get; private set; }


    public void Dispose()
    {
        if (_disposed)
        {
            return;
        }
        _disposed = true;

        try
        {
            _stopwatch.Stop();
            _reportAction.Invoke(_stopwatch.Elapsed);
        }
        catch (Exception ex)
        {
            DisposingException = ex;
        }
    }

    public static ElapsedTimeReporter Start(Action<TimeSpan> reportAction)
    {
        var reporter = new ElapsedTimeReporter(Stopwatch.StartNew(), reportAction);

        return reporter;
    }

}
