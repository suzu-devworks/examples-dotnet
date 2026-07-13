using Microsoft.Extensions.Time.Testing;

namespace Examples.Caching.Tests;

/// <summary>
/// Tests <see cref="CacheProxy{T}" /> class.
/// </summary>
public class CacheProxyTests
{
    private record Sample(DateTimeOffset CreatedAt);

    [Fact]
    public void When_ExceedingHoldPeriod_Then_CacheItemIsRefreshed()
    {
        var fakeTimeProvider = new FakeTimeProvider(new DateTimeOffset(2026, 1, 1, 0, 0, 0, TimeSpan.Zero));

        var cache = new CacheProxy<Sample>(
                TimeSpan.FromSeconds(100),
                () => new(DateTimeOffset.Now),
                fakeTimeProvider);

        var instance1 = cache.Get();
        var instance2 = cache.Get();

        fakeTimeProvider.Advance(TimeSpan.FromSeconds(101));

        var instance3 = cache.Get();
        var instance4 = cache.Get();

        Assert.Same(instance1, instance2);
        Assert.NotSame(instance1, instance3);
        Assert.Same(instance3, instance4);
    }
}
