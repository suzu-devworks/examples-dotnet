using System.Collections.Concurrent;

namespace Examples.Concurrency;

/// <summary>
/// A wrapper (aggregator) for operating multiple low-level threads at once.
/// </summary>
/// <remarks>
/// <para>Because the management thread is not necessarily a separate thread.</para>
/// </remarks>
public sealed class ThreadAggregator
{
    private readonly ConcurrentQueue<(Action WorkAction, Action CompletionAction)> _workers = new();
    private readonly int _maxThreads;
    private readonly List<WeakReference<Thread>> _threads = new();
    private readonly ConcurrentQueue<Exception> _exceptions = new();
    private int _runningCount;
    private int _completedCount;

    /// <summary>
    /// Initializes a new <see cref="ThreadAggregator" /> instance with the specified maximum value.
    /// </summary>
    /// <param name="maxThreads">The number of threads maximum value.</param>
    public ThreadAggregator(int maxThreads)
    {
        _maxThreads = maxThreads;
    }

    /// <summary>
    /// Gets the number of completed threads.
    /// </summary>
    public int CompletedCount => _completedCount;

    /// <summary>
    /// Gets the number of running threads.
    /// </summary>
    public int RunningCount => _runningCount;

    /// <summary>
    /// Gets the exceptions that occurred during thread execution.
    /// </summary>
    public IReadOnlyList<Exception> Exceptions => _exceptions.ToList();

    /// <summary>
    /// Adds a worker thread to the aggregator.
    /// </summary>
    /// <param name="workAction"></param>
    /// <param name="completionAction"></param>
    /// <returns></returns>
    public ThreadAggregator AddWorker(Action workAction, Action completionAction)
    {
        _workers.Enqueue((workAction, completionAction));
        return this;
    }

    /// <summary>
    /// Starts all threads simultaneously.
    /// </summary>
    public void StartAll()
    {
        var startSignal = new ManualResetEventSlim(false);

        for (int i = 0; i < _maxThreads; i++)
        {
            var thread = new Thread(() =>
            {
                startSignal.Wait();
                var currentThreadId = Environment.CurrentManagedThreadId;

                while (_workers.TryDequeue(out var worker))
                {
                    try
                    {
                        Interlocked.Increment(ref _runningCount);

                        worker.WorkAction();
                        worker.CompletionAction();

                        Interlocked.Increment(ref _completedCount);
                    }
                    catch (Exception ex)
                    {
                        _exceptions.Enqueue(new ApplicationException(
                                $"Thread {currentThreadId} encountered an exception.", ex));
                    }
                    finally
                    {
                        Interlocked.Decrement(ref _runningCount);
                    }
                }
            });

            _threads.Add(new(thread));
            thread.Start();
        }

        startSignal.Set();
    }

    /// <summary>
    /// Waits for all threads to complete and blocks the current thread.
    /// </summary>
    public void WaitAll()
    {
        _threads.ForEach(weak =>
        {
            if (weak.TryGetTarget(out var thread))
            {
                thread.Join();
            }
        });
        _threads.Clear();
    }
}
