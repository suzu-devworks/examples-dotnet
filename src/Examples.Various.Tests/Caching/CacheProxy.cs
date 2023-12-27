namespace Examples.Caching;

public class CacheProxy<T>
{
    private T? _cacheItem;
    private readonly Func<T> _picker;
    private readonly object _readLocked = new();
    private readonly object _writeLocked = new();
    public TimeSpan HoldPeriod { get; }
    public DateTime? ExpiredDateTime { get; private set; }

    public CacheProxy(TimeSpan holdPeriod, Func<T> picker)
    {
        HoldPeriod = holdPeriod;
        _picker = picker;
    }

    public T Get()
    {
        if (_cacheItem is null || ExpiredDateTime < DateTime.Now)
        {
            var oldItem = _cacheItem;

            lock (_writeLocked)
            {
                if (_cacheItem is null || ExpiredDateTime < DateTime.Now)
                {
                    var item = _picker();

                    lock (_readLocked)
                    {
                        _cacheItem = item;
                        // TODO　When is updated regularly, you can't even want to refresh.
                        ExpiredDateTime = DateTime.Now + HoldPeriod;
                    }

                }
            }

            // object Disposed support.
            (oldItem as IDisposable)?.Dispose();
        }
        lock (_readLocked)
        {
            return _cacheItem;
        }
    }
}
