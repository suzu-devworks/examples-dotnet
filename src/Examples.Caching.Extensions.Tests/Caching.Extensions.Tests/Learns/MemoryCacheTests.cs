
using Examples.Caching.Extensions.Tests.Fixtures;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Time.Testing;

namespace Examples.Caching.Extensions.Tests.Learns;

/// <summary>
/// Tests to study how to use and test <see cref="MemoryCache" />.
/// </summary>
/// <seealso href="https://docs.microsoft.com/ja-jp/dotnet/core/extensions/caching" />
public class MemoryCacheTests
{
    private record Sample(int? Number);

    [Fact]
    public void When_UsingAbsoluteExpiration_Then_CacheExpiresBasedOnAbsoluteTime()
    {
        var fakeTimeProvider = new FakeTimeProvider(new DateTimeOffset(2026, 1, 1, 0, 0, 0, TimeSpan.Zero));
        var clock = new TimeProviderClockAdapter(fakeTimeProvider);

        using IMemoryCache cache = new MemoryCache(new MemoryCacheOptions { Clock = clock });

        // Register Sample to cache.
        var originalInstance = new Sample(Number: 100);
        cache.Set("Foo", originalInstance, new MemoryCacheEntryOptions()
        {
            // Set absolute expiration to 2026-01-01 20:00:00 UTC.
            AbsoluteExpiration = new DateTimeOffset(2026, 1, 1, 20, 0, 0, TimeSpan.Zero)
        });

        // Get cache.
        fakeTimeProvider.SetUtcNow(new DateTimeOffset(2026, 1, 1, 19, 59, 59, TimeSpan.Zero));

        var validInstance = cache.Get<Sample>("Foo");

        Assert.NotNull(validInstance);
        Assert.Same(originalInstance, validInstance);

        // Expire cache.
        fakeTimeProvider.SetUtcNow(new DateTimeOffset(2026, 1, 1, 20, 0, 0, TimeSpan.Zero));

        var expiredInstance = cache.Get<Sample>("Foo");
        Assert.Null(expiredInstance);
    }

    [Fact]
    public void When_UsingAbsoluteExpirationRelativeToNow_Then_CacheExpiresBasedOnRelativeTime()
    {
        var fakeTimeProvider = new FakeTimeProvider(new DateTimeOffset(2026, 1, 1, 0, 0, 0, TimeSpan.Zero));
        var clock = new TimeProviderClockAdapter(fakeTimeProvider);

        using IMemoryCache cache = new MemoryCache(new MemoryCacheOptions { Clock = clock });

        // Register Sample to cache.
        var originalInstance = new Sample(Number: 200);
        cache.Set("Foo", originalInstance, new MemoryCacheEntryOptions()
        {
            // Set absolute expiration relative to now to 20 minutes.
            AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(20)
        });

        // Get cache.
        fakeTimeProvider.Advance(TimeSpan.FromMinutes(19).Add(TimeSpan.FromSeconds(59)));

        var validInstance = cache.Get<Sample>("Foo");

        Assert.NotNull(validInstance);
        Assert.Same(originalInstance, validInstance);

        // Expire cache.
        fakeTimeProvider.Advance(TimeSpan.FromSeconds(1));

        var expiredInstance = cache.Get<Sample>("Foo");
        Assert.Null(expiredInstance);
    }

    [Fact]
    public void When_UsingSlidingExpiration_Then_CacheExpiresBasedOnSlidingTime()
    {
        var fakeTimeProvider = new FakeTimeProvider(new DateTimeOffset(2026, 1, 1, 0, 0, 0, TimeSpan.Zero));
        var clock = new TimeProviderClockAdapter(fakeTimeProvider);

        using IMemoryCache cache = new MemoryCache(new MemoryCacheOptions { Clock = clock });

        // Register Sample to cache.
        var originalInstance = new Sample(Number: 300);
        cache.Set("Foo", originalInstance, new MemoryCacheEntryOptions()
        {
            // Set sliding expiration to 20 minutes.
            SlidingExpiration = TimeSpan.FromMinutes(20)
        });

        // Get cache.
        fakeTimeProvider.Advance(TimeSpan.FromMinutes(19).Add(TimeSpan.FromSeconds(59)));

        var validInstance = cache.Get<Sample>("Foo");

        Assert.NotNull(validInstance);
        Assert.Same(originalInstance, validInstance);

        // Get cache(sliding).
        fakeTimeProvider.Advance(TimeSpan.FromSeconds(1));

        var validInstance2 = cache.Get<Sample>("Foo");

        Assert.NotNull(validInstance2);
        Assert.Same(originalInstance, validInstance2);

        // Expire cache.
        fakeTimeProvider.Advance(TimeSpan.FromMinutes(20));

        var expiredInstance = cache.Get<Sample>("Foo");
        Assert.Null(expiredInstance);
    }

    [Fact]
    public void When_UsingRegisterPostEvictionCallback_Then_CanHookRemoveOperation()
    {
        var fakeTimeProvider = new FakeTimeProvider(new DateTimeOffset(2026, 1, 1, 0, 0, 0, TimeSpan.Zero));
        var clock = new TimeProviderClockAdapter(fakeTimeProvider);

        using IMemoryCache cache = new MemoryCache(new MemoryCacheOptions { Clock = clock });

        void OnPostEviction(object key, object? value, EvictionReason reason, object? state)
        {
            if (value is Sample sample)
            {
                Console.WriteLine($"{sample.Number} was evicted for {reason}.");
            }
        }

        // Register Sample to cache.
        var originalInstance = cache.GetOrCreate<Sample>("Foo", entry =>
        {
            entry.SetPriority(CacheItemPriority.NeverRemove);
            entry.SetAbsoluteExpiration(fakeTimeProvider.GetUtcNow().AddHours(1.0));
            entry.RegisterPostEvictionCallback(OnPostEviction, state: null);

            return new(Number: 400);
        });

        var validInstance = cache.Get<Sample>("Foo");

        Assert.NotNull(validInstance);
        Assert.Same(originalInstance, validInstance);

        fakeTimeProvider.Advance(TimeSpan.FromHours(1.0));

        var expiredInstance = cache.Get<Sample>("Foo");

        Assert.Null(expiredInstance);
    }

    [Fact]
    public void When_UsingSizeLimitAndCacheItemPriority_Then_CanControlPriorityOfRemove()
    {
        using var cache = new MemoryCache(new MemoryCacheOptions { SizeLimit = 3 });

        // Register Sample to cache.
        cache.Set("Foo", new Sample(Number: 500), new MemoryCacheEntryOptions()
            .SetSize(1).SetPriority(CacheItemPriority.Low));
        cache.Set("Bar", new Sample(Number: 501), new MemoryCacheEntryOptions()
            .SetSize(1).SetPriority(CacheItemPriority.Normal));
        cache.Set("Baz", new Sample(Number: 502), new MemoryCacheEntryOptions()
            .SetSize(1).SetPriority(CacheItemPriority.High));

        Assert.Equal(3, cache.Count);

        // Set Qux.
        cache.Set("Qux", new Sample(Number: 503), new MemoryCacheEntryOptions()
            .SetSize(1).SetPriority(CacheItemPriority.High));

        Assert.Equal(3, cache.Count);

        Assert.NotNull(cache.Get<Sample>("Foo"));
        Assert.NotNull(cache.Get<Sample>("Bar"));
        Assert.NotNull(cache.Get<Sample>("Baz"));
        // It is not the case that Quz takes precedence over Foo.
        Assert.Null(cache.Get<Sample>("Quz"));

        // The priority changes when compressing.
        cache.Compact(0.5);

        Assert.Equal(2, cache.Count);

        Assert.Null(cache.Get<Sample>("Foo")); //removed
        Assert.NotNull(cache.Get<Sample>("Bar"));
        Assert.NotNull(cache.Get<Sample>("Baz"));
    }

    [Fact]
    public void When_SpecifyingPercentageForCompact_Then_CanControlNumberOfItemToBeRemove()
    {
        using var cache = new MemoryCache(new MemoryCacheOptions { SizeLimit = 100 });

        void OnPostEviction(object key, object? value, EvictionReason reason, object? state)
        {
            if (value is Sample sample)
            {
                Console.WriteLine($"{sample.Number} was evicted for {reason}.");
            }
        }

        // Register Sample to cache.
        var option = new MemoryCacheEntryOptions()
            .SetSize(1).RegisterPostEvictionCallback(OnPostEviction);
        foreach (var i in Enumerable.Range(0, 100))
        {
            cache.Set($"Key:{i}", new Sample(Number: 1000 + i), option);
        }

        Assert.Equal(100, cache.Count);

        // 100 - (1/100) = 99.0
        cache.Compact(0.01);
        Assert.Equal(99, cache.Count);

        // 99 - (99 * 10/100) = 89.1
        cache.Compact(0.1);
        Assert.Equal(90, cache.Count);

        // 90 - (90 * 20/100) = 72.0
        cache.Compact(0.2);
        Assert.Equal(72, cache.Count);

        // 72 - (72 * 25/100) = 54.0
        cache.Compact(0.25);
        Assert.Equal(54, cache.Count);

        // 54 - (54 * 33/100) = 36.18
        cache.Compact(0.33);
        Assert.Equal(37, cache.Count);

        // 37 - (37 * 50/100) = 18.5
        cache.Compact(0.5);
        Assert.Equal(19, cache.Count);

        // 19 - (19 * 100/100) = 0
        cache.Compact(1.0);
        Assert.Equal(0, cache.Count);
    }
}
