using Xunit.Sdk;

namespace Examples.Xunit;

/// <summary>
/// A wrapper (aggregator) for operating multiple low-level threads at once.
/// </summary>
/// <remarks>
/// <para>Because the management thread is not necessarily a separate thread.</para>
/// </remarks>
public sealed class ThreadAggregator
{
    private readonly List<WeakReference<Thread>> _threads;

    /// <summary>
    /// Initializes a new <see cref="ThreadAggregator" /> instance with the default maximum value.
    /// </summary>
    public ThreadAggregator() : this(maxThreads: 4)
    {
    }

    /// <summary>
    /// Initializes a new <see cref="ThreadAggregator" /> instance with the specified maximum value.
    /// </summary>
    /// <param name="maxThreads">The number of threads maximum value.</param>
    public ThreadAggregator(int maxThreads)
    {
        _threads = new(capacity: maxThreads);
    }

    /// <summary>
    /// Create aggregate thread instance.
    /// </summary>
    /// <param name="action">The method that executes on a thread.</param>
    /// <returns>A <see cref="Thread" /> instance.</returns>
    public Thread CreateNew(ThreadStart action)
    {
        if (_threads.Count == _threads.Capacity)
        {
            throw new XunitException($"The number of threads to aggregate has overflowed! : {_threads.Capacity}");
        }

        var thread = new Thread(action);
        _threads.Add(new(thread));
        return thread;
    }

    /// <summary>
    /// Create aggregate thread instance and start now.
    /// </summary>
    /// <param name="action">The method that executes on a thread.</param>
    /// <returns>A <see cref="Thread" /> instance.</returns>
    public Thread StartNew(ThreadStart action)
    {
        var thread = CreateNew(action);
        thread.Start();
        return thread;
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
