namespace Examples.Caching.Tests;

/// <summary>
/// Tests <see cref="CacheProxy{T}" /> class.
/// </summary>
public class CacheProxyTests
{

    [Fact]
    public void WhenCallingSetAndGet()
    {
        var cache = new CacheProxy<Sample>(TimeSpan.FromMilliseconds(100)
                                , () => new() { Created = DateTimeOffset.Now });
        var instance1 = cache.Get();
        var instance2 = cache.Get();
        Thread.Sleep(200);
        var instance3 = cache.Get();
        var instance4 = cache.Get();

        (instance1 == instance2).IsTrue();
        (instance1 == instance3).IsFalse();
        (instance3 == instance4).IsTrue();

        return;
    }

    private class Sample
    {
        public DateTimeOffset Created { get; init; }
    }

}
