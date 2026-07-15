using System.Runtime.Caching;
using Examples.Caching.Runtime.Tests.Fixtures;

namespace Examples.Caching.Runtime.Tests.Learns;

/// <summary>
/// Tests to study how to use and test <see cref="MemoryCache" />.
/// </summary>
public class MemoryCacheTests
{
    private record Sample(int? Number);

    [Fact]
    public void When_UsingAbsoluteExpiration_Then_CacheExpiresBasedOnAbsoluteTime()
    {
        // Arrange.
        var originalInstance = new Sample(Number: 100);

        using var cache = new MemoryCache("AbsoluteExpiration Test");

        cache.Add("Foo", originalInstance, new CacheItemPolicy()
        {
            AbsoluteExpiration = DateTime.UtcNow.AddHours(1) /* dummy */
        });

        var accessor = cache.GetEntryAccessor("Foo");

        // https://github.com/dotnet/runtime/blob/main/src/libraries/System.Runtime.Caching/src/System/Runtime/Caching/MemoryCacheStore.cs#L292
        // if (entry != null && entry.UtcAbsExp <= DateTime.UtcNow)

        // Act and Assert.
        var validInstance = cache["Foo"];

        Assert.NotNull(validInstance);
        Assert.Same(originalInstance, validInstance);

        accessor.UtcAbsExp = DateTime.UtcNow.AddSeconds(-1.0);

        var expiredInstance = cache["Foo"];

        Assert.Null(expiredInstance);
    }

    [Fact]
    public async Task When_UsingSlidingExpiration_Then_CacheExpiresBasedOnSlidingTime()
    {
        // Arrange.
        var originalInstance = new Sample(200);

        using var cache = new MemoryCache("SlidingExpiration Test");

        cache.Add("Foo", originalInstance, new CacheItemPolicy()
        {
            SlidingExpiration = TimeSpan.FromHours(1) /* dummy */
        });

        var accessor = cache.GetEntryAccessor("Foo");

        // https://github.com/dotnet/runtime/blob/main/src/libraries/System.Runtime.Caching/src/System/Runtime/Caching/MemoryCacheStore.cs#L292
        // internal MemoryCacheEntry Get(MemoryCacheKey key)
        // if (entry != null && entry.UtcAbsExp <= DateTime.UtcNow) ... remove

        // https://github.com/dotnet/runtime/blob/main/src/libraries/System.Runtime.Caching/src/System/Runtime/Caching/MemoryCacheStore.cs#L130
        // internal void UpdateExpAndUsage(MemoryCacheEntry entry, bool updatePerfCounters = true)

        // https://github.com/dotnet/runtime/blob/main/src/libraries/System.Runtime.Caching/src/System/Runtime/Caching/MemoryCacheEntry.cs#L291
        // internal void UpdateSlidingExp(DateTime utcNow, CacheExpires expires)
        // DateTime utcNewExpires = utcNow + _slidingExp;
        // if (utcNewExpires - _utcAbsExp >= CacheExpires.MIN_UPDATE_DELTA || utcNewExpires < _utcAbsExp)

        // https://github.com/dotnet/runtime/blob/main/src/libraries/System.Runtime.Caching/src/System/Runtime/Caching/CacheExpires.cs#L760
        // internal static readonly TimeSpan MIN_UPDATE_DELTA = new TimeSpan(0, 0, 1);

        // Act
        var beforeExp = accessor.UtcAbsExp;

        await Task.Delay(1000, TestContext.Current.CancellationToken);
        var validInstance = cache["Foo"];
        var afterExp = accessor.UtcAbsExp;

        Assert.NotNull(validInstance);
        Assert.Same(originalInstance, validInstance);
        Assert.True(afterExp > beforeExp,
            $"afterExp:{afterExp:HH:mm:ss.fffZ} should be greater than beforeExp:{beforeExp:HH:mm:ss.fffZ}");

        accessor.UtcAbsExp = DateTime.UtcNow.AddSeconds(-1.0);

        var expiredInstance = cache["Foo"];

        Assert.Null(expiredInstance);
    }
}
