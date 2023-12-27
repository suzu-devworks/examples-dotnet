using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Internal;

namespace Examples.Caching.Tests._.Microsoft.Extensions.Caching;

/// <summary>
/// Tests to study how to use and test <see cref="MemoryCache" />.
/// </summary>
/// <seealso href="https://docs.microsoft.com/ja-jp/dotnet/core/extensions/caching" />
public partial class MemoryCacheTests
{

    [Fact]
    public void WhenCallingSetAndGet_WithAbsoluteExpiration()
    {
        var testExpiredDateTime = DateTime.UtcNow;

        // Set fake expire system clock.
        var mock = new Mock<ISystemClock>();
        mock.SetupSequence(x => x.UtcNow)
            .Returns(() => testExpiredDateTime.AddSeconds(0.0))     // ctor.
            .Returns(() => testExpiredDateTime.AddSeconds(0.0))     // Set().
            .Returns(() => testExpiredDateTime.AddSeconds(19.9))
            .Returns(() => testExpiredDateTime.AddSeconds(20.0))
            //.Returns(() => testExpiredDateTime.AddSeconds(20.1))
            ;

        using var cache = new MemoryCache(new MemoryCacheOptions { Clock = mock.Object });

        // register Sample to cache.
        var baseInstance = new Sample { Number = 100 };
        var option = new MemoryCacheEntryOptions()
            .SetAbsoluteExpiration(TimeSpan.FromSeconds(20.0));

        cache.Set("Foo", baseInstance, option);

        //Get cache.
        cache.Get<Sample>("Foo").Is(baseInstance, "current < expired.");
        cache.Get<Sample>("Foo").IsNull("current == expired.");
        //cache.Get<Sample>("Foo").IsNull("current > expired.");

        // Assert.
        mock.VerifyGet(x => x.UtcNow, Times.Exactly(4));
        mock.VerifyNoOtherCalls();

        return;
    }

    [Fact]
    public void WhenCallingSetAndGet_WithSlidingExpiration()
    {
        var testExpiredDateTime = DateTime.UtcNow;

        // Set fake expire system clock.
        var mock = new Mock<ISystemClock>();
        mock.SetupSequence(x => x.UtcNow)
            .Returns(() => testExpiredDateTime.AddSeconds(0.0))       // ctor.
            .Returns(() => testExpiredDateTime.AddSeconds(0.0))       // Set().
            .Returns(() => testExpiredDateTime.AddSeconds(19.9))
            .Returns(() => testExpiredDateTime.AddSeconds(19.9 + 19.9))
            .Returns(() => testExpiredDateTime.AddSeconds(19.9 + 19.9 + 20.0))
            //.Returns(() => testExpiredDateTime.AddSeconds(19.9 + 19.9 + 20.0 + 20.1))
            ;

        using var cache = new MemoryCache(new MemoryCacheOptions { Clock = mock.Object });

        // register Sample to cache.
        var baseInstance = new Sample { Number = 200 };
        var option = new MemoryCacheEntryOptions()
            .SetSlidingExpiration(TimeSpan.FromSeconds(20.0));

        cache.Set("Foo", baseInstance, option);

        //Get cache.
        cache.Get<Sample>("Foo").Is(baseInstance, "previous - current < expired time.");
        cache.Get<Sample>("Foo").Is(baseInstance, "previous - current < expired time.");
        cache.Get<Sample>("Foo").IsNull("previous - current = expired time.");
        //cache.Get<Sample>("Foo").IsNull("previous - current > expired time.");

        mock.VerifyGet(x => x.UtcNow, Times.Exactly(5));
        mock.VerifyNoOtherCalls();

        return;
    }

    [Fact]
    public void WhenCallingSetAndGet_WithPrioritySizeLimit()
    {
        using var cache = new MemoryCache(new MemoryCacheOptions { SizeLimit = 3 });

        // register Sample to cache.
        var baseInstance = new Sample { Number = 300 };
        var option = new MemoryCacheEntryOptions()
            .SetSize(1);

        cache.Set("Foo", baseInstance, option.SetPriority(CacheItemPriority.Normal));

        //Get cache.
        cache.Get<Sample>("Foo").Is(baseInstance, "Count == 1.");

        cache.Count.Is(1);
        cache.Set("Bar1", new Sample { Number = 301 }, option.SetPriority(CacheItemPriority.High));

        cache.Count.Is(2);
        cache.Set("Bar2", new Sample { Number = 302 }, option.SetPriority(CacheItemPriority.High));

        cache.Count.Is(3);
        cache.Compact(0.34);
        cache.Set("Bar3", new Sample { Number = 303 }, option.SetPriority(CacheItemPriority.High));
        cache.Count.Is(3);
        cache.Get<Sample>("Bar3").IsNotNull();
        cache.Get<Sample>("Bar2").IsNotNull();
        cache.Get<Sample>("Bar1").IsNotNull();
        cache.Get<Sample>("Foo").IsNull();

        return;
    }

    [Fact]
    public void WhenCallingCompact()
    {
        using var cache = new MemoryCache(new MemoryCacheOptions { SizeLimit = 100 });

        // register Sample to cache.
        var baseInstance = new Sample { Number = 400 };
        var option = new MemoryCacheEntryOptions()
            .SetSize(1);

        foreach (var i in Enumerable.Range(0, 100))
        {
            cache.Set($"Key:{i}", baseInstance, option);
        }

        cache.Count.Is(100);

        // 100 - (1/100) = 99.0
        cache.Compact(0.01);
        cache.Count.Is(99);

        // 99 - (99 * 10/100) = 89.1
        cache.Compact(0.1);
        cache.Count.Is(90);

        // 90 - (90 * 20/100) = 72.0
        cache.Compact(0.2);
        cache.Count.Is(72);

        // 72 - (72 * 25/100) = 54.0
        cache.Compact(0.25);
        cache.Count.Is(54);

        // 54 - (54 * 33/100) = 36.18
        cache.Compact(0.33);
        cache.Count.Is(37);

        // 37 - (37 * 50/100) = 18.5
        cache.Compact(0.5);
        cache.Count.Is(19);

        // 19 - (19 * 100/100) = 0
        cache.Compact(1.0);
        cache.Count.Is(0);

        return;
    }

    [Fact]
    public void WhenCallingRemovedCallback()
    {
        var mockVerifier = new Mock<IVerifier>();
        mockVerifier.Setup(x => x.Called("key: Baz"));
        mockVerifier.Setup(x => x.Called("value: 500"));
        mockVerifier.Setup(x => x.Called("reason: Expired"));
        mockVerifier.Setup(x => x.Called($"state: {nameof(MemoryCacheTests)}"));

        var verifier = mockVerifier.Object;
        using var waitEvent = new AutoResetEvent(false);

        var testExpiredDateTime = DateTime.UtcNow;

        // Set fake expire system clock.
        var mock = new Mock<ISystemClock>();
        mock.SetupSequence(x => x.UtcNow)
            .Returns(() => testExpiredDateTime.AddSeconds(0.0))       // ctor.
            .Returns(() => testExpiredDateTime.AddSeconds(0.0))       // GetOrCreate().
            .Returns(() => testExpiredDateTime.AddSeconds(0.0))       // GetOrCreate().
            .Returns(() => testExpiredDateTime.AddSeconds(19.9))
            .Returns(() => testExpiredDateTime.AddSeconds(20.0))
            ;

        using var cache = new MemoryCache(new MemoryCacheOptions { Clock = mock.Object });

        // register Sample to cache.
        var baseInstance = cache.GetOrCreate<Sample>("Baz", entry =>
        {
            entry.SetPriority(CacheItemPriority.NeverRemove);
            entry.SetAbsoluteExpiration(testExpiredDateTime.AddSeconds(20.0));
            entry.RegisterPostEvictionCallback(callback: (key, value, reason, state) =>
            {
                verifier.Called($"key: {key}");
                verifier.Called($"value: {((Sample?)value)?.Number}");
                verifier.Called($"reason: {reason}");
                verifier.Called($"state: {state?.GetType().Name}");

                //It runs in different threads, so you need to align the timing.
                waitEvent.Set();
            },
            state: this);

            return new Sample { Number = 500 };
        });

        //Get cache.
        cache.Get<Sample>("Baz").Is(baseInstance, "current < expired.");
        cache.Get<Sample>("Baz").IsNull("current == expired.");
        //cache.Get<Sample>("Baz").IsNull("current > expired.");

        mock.VerifyGet(x => x.UtcNow, Times.Exactly(5));
        mock.VerifyNoOtherCalls();

        waitEvent.WaitOne();
        mockVerifier.Verify(x => x.Called("key: Baz"), Times.Once);
        mockVerifier.Verify(x => x.Called("value: 500"), Times.Once);
        mockVerifier.Verify(x => x.Called("reason: Expired"), Times.Once);
        mockVerifier.Verify(x => x.Called($"state: {nameof(MemoryCacheTests)}"), Times.Once);
        mockVerifier.VerifyNoOtherCalls();

        return;
    }

}
