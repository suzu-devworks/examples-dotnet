namespace Examples.Caching;

public class CacheProxy<T>
{
    private T? _cacheItem;
    private readonly Func<T> _picker;
    private readonly TimeProvider _timeProvider;
    private readonly object _readLocked = new();
    private readonly object _writeLocked = new();

    public CacheProxy(TimeSpan holdPeriod, Func<T> picker, TimeProvider? timeProvider = null)
    {
        HoldPeriod = holdPeriod;
        _picker = picker;
        _timeProvider = timeProvider ?? TimeProvider.System;
    }

    public TimeSpan HoldPeriod { get; }
    public DateTimeOffset? ExpiredDateTime { get; private set; }

    public T Get()
    {
        var now = _timeProvider.GetUtcNow();
        if (_cacheItem is null || ExpiredDateTime < now)
        {
            lock (_writeLocked)
            {
                if (_cacheItem is null || ExpiredDateTime < now)
                {
                    var oldItem = _cacheItem;

                    var item = _picker();

                    lock (_readLocked)
                    {
                        _cacheItem = item;
                        // TODO　When is updated regularly, you can't even want to refresh.
                        ExpiredDateTime = now + HoldPeriod;
                    }

                    // object Disposed support.
                    (oldItem as IDisposable)?.Dispose();
                }
            }

        }
        lock (_readLocked)
        {
            return _cacheItem;
        }
    }
}
