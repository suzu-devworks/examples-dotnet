using System.Runtime.Caching;

namespace Examples.Caching.Tests._.System.Runtime.Caching;

/// <summary>
/// Tests to study how to use and test <see cref="MemoryCache" />.
/// </summary>
public partial class MemoryCacheTests(ITestOutputHelper output)
{
    private readonly ITestOutputHelper _output = output;

    [Fact]
    public void WhenCallingSetAndGet_WithAbsoluteExpiration()
    {
        // Arrange.
        using var cache = new MemoryCache("TEST1");

        var instance = new Sample { Number = 100 };

        cache.Add("Foo", instance, new CacheItemPolicy()
        {
            AbsoluteExpiration = DateTimeOffset.Now + TimeSpan.FromMinutes(10.0)
        });

        // Act and Asset entry.UtcAbsExp > DateTime.UtcNow
        cache.SetAbsoluteExpiration("Foo", DateTime.UtcNow + TimeSpan.FromMilliseconds(100.0));
        cache["Foo"].Is(instance);

        // Act and Asset entry.UtcAbsExp <= DateTime.UtcNow
        cache.SetAbsoluteExpiration("Foo", DateTime.UtcNow - TimeSpan.FromMilliseconds(100.0));
        cache["Foo"].IsNull();

        return;
    }

    [Fact]
    public void WhenCallingSetAndGet_WithSlidingExpiration()
    {
        // Arrange.
        using var cache = new MemoryCache("TEST2");

        var instance = new Sample { Number = 200 };

        cache.Add("Foo", instance, new CacheItemPolicy()
        {
            SlidingExpiration = TimeSpan.FromSeconds(10.0)
        });

        // Act and Asset sliding.
        //  if (utcNewExpires - _utcAbsExp >= CacheExpires.MIN_UPDATE_DELTA || utcNewExpires < _utcAbsExp)
        //  MIN_UPDATE_DELTA = new TimeSpan(0, 0, 1);

        for (var i = 5; i >= 0; i--)
        {
            //Thread.Sleep(1000);
            cache.SetAbsoluteExpiration("Foo", DateTime.UtcNow + TimeSpan.FromSeconds(i) + TimeSpan.FromSeconds(0.1));

            var before = cache.GetAbsoluteExpiration("Foo");
            cache["Foo"].Is(instance);
            var after = cache.GetAbsoluteExpiration("Foo");

            before.IsNot(after);

            var now = DateTime.Now;
            _output.WriteLine($"[{i,2}]{now:HH:mm:ss.fff}: {before:HH:mm:ss.fff} -> {after:HH:mm:ss.fff}");
        }

        // Asset entry.UtcAbsExp <= DateTime.UtcNow
        cache.SetAbsoluteExpiration("Foo", DateTime.UtcNow - TimeSpan.FromMilliseconds(100.0));
        cache["Foo"].IsNull();

        return;
    }

}
