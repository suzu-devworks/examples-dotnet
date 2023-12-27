using System.Runtime.Caching;
using Xunit.Abstractions;

namespace Examples.Caching.Tests._.System.Runtime.Caching;

/// <summary>
/// Tests to study how to use and test <see cref="MemoryCache" />.
/// </summary>
public partial class MemoryCacheTests(ITestOutputHelper output)
{
    // ```
    // dotnet test --logger "console;verbosity=detailed"
    // ```

    [Fact]
    public void WhenCallingGet_WithAbsoluteExpiration_ReturnsNullForExpired()
    {
        // Arrange.
        var data = new Sample { Number = 100 };

        using var cache = new MemoryCache("AbsoluteExpiration Test")
        {
            {
                "Foo",
                data,
                new CacheItemPolicy()
                {
                    AbsoluteExpiration = DateTime.UtcNow.AddHours(1) /* dummy */
                }
            },
            {
                "Bar",
                data,
                new CacheItemPolicy()
                {
                    AbsoluteExpiration = DateTime.UtcNow.AddHours(1) /* dummy */
                }
            },
            {
                "Baz",
                data,
                new CacheItemPolicy()
                {
                    AbsoluteExpiration = DateTime.UtcNow.AddHours(1) /* dummy */
                }
            }
        };

        var accessorFoo = cache.GetEntryAccessor("Foo");
        var accessorBar = cache.GetEntryAccessor("Bar");
        var accessorBaz = cache.GetEntryAccessor("Baz");

        // Act
        // https://github.com/dotnet/runtime/blob/main/src/libraries/System.Runtime.Caching/src/System/Runtime/Caching/MemoryCacheStore.cs#L292
        // if (entry != null && entry.UtcAbsExp <= DateTime.UtcNow)
        accessorFoo.UtcAbsExp = DateTime.UtcNow.AddMilliseconds(-100.0);

        output.WriteLine($"before: {DateTime.UtcNow:HH:mm:ss.fffZ}");
        var beforeFoo = cache["Foo"];
        var beforeBar = cache["Bar"];
        var beforeBaz = cache["Baz"];

        accessorBar.UtcAbsExp = DateTime.UtcNow.AddMilliseconds(-100.0);

        output.WriteLine($"after: {DateTime.UtcNow:HH:mm:ss.fffZ}");
        var afterFoo = cache["Foo"];
        var afterBar = cache["Bar"];
        var afterBaz = cache["Baz"];

        // Assert
        beforeFoo.IsNull();
        beforeBar.Is(data);
        beforeBaz.Is(data);

        afterFoo.IsNull();
        afterBar.IsNull();
        afterBaz.Is(data);

        return;
    }

    [Fact]
    public async Task WhenCallingGet_WithSlidingExpiration_ReturnsNullForExpired()
    {
        // Arrange.
        var data = new Sample { Number = 200 };

        using var cache = new MemoryCache("SlidingExpiration Test")
          {
            {
                "Foo",
                data,
                new CacheItemPolicy()
                {
                    SlidingExpiration = TimeSpan.FromHours(1) /* dummy */
                }
            },
            {
                "Bar",
                data,
                new CacheItemPolicy()
                {
                    SlidingExpiration = TimeSpan.FromHours(1) /* dummy */
                }
            },
            {
                "Baz",
                data,
                new CacheItemPolicy()
                {
                    SlidingExpiration = TimeSpan.Zero /* non expired. */
                }
            }
        };

        var accessorFoo = cache.GetEntryAccessor("Foo");
        var accessorBar = cache.GetEntryAccessor("Bar");
        var accessorBaz = cache.GetEntryAccessor("Baz");

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

        static DateTime Get(MemoryCacheExtensions.MemoryCacheEntryAccessor accessor, DateTime utcNow, Action<DateTime, DateTime, TimeSpan> reporter)
        {
            var utcAbsExp = accessor.UtcAbsExp;
            var utcNewExpires = utcNow + accessor.SlidingExp;
            var remainingTime = utcNewExpires - utcAbsExp;
            reporter.Invoke(utcAbsExp, utcNewExpires, remainingTime);

            return utcAbsExp;
        }

        // Act

        accessorFoo.UtcAbsExp = DateTime.UtcNow + TimeSpan.FromMilliseconds(100);
        accessorBar.UtcAbsExp = DateTime.UtcNow + TimeSpan.FromMilliseconds(100);

        var beforeExpFoo = Get(accessorFoo, DateTime.Now, (a, n, r) =>
            output.WriteLine($"before:Foo {a:HH:mm:ss.fffZ}, next:{n:HH:mm:ss.fffZ}, {r} is {r >= TimeSpan.FromSeconds(1)}")
        );
        var beforeExpBar = Get(accessorBar, DateTime.Now, (a, n, r) =>
            output.WriteLine($"before:Bar {a:HH:mm:ss.fffZ}, next:{n:HH:mm:ss.fffZ}, {r} is {r >= TimeSpan.FromSeconds(1)}")
        );
        var beforeExpBaz = Get(accessorBaz, DateTime.UtcNow, (a, n, _) =>
            output.WriteLine($"before:Baz {a:HH:mm:ss.fffZ}")
        );

        output.WriteLine($"get:       {DateTime.UtcNow:HH:mm:ss.fffZ}");
        //var beforeFoo = cache["Foo"];
        var beforeBar = cache["Bar"];   // sliding expiration.
        var beforeBaz = cache["Baz"];   // no-sliding expiration.

        await Task.Delay(100);

        output.WriteLine($"get:       {DateTime.UtcNow:HH:mm:ss.fffZ}");
        var afterFoo = cache["Foo"];    // expired.
        var afterBar = cache["Bar"];    // sliding expiration.
        var afterBaz = cache["Baz"];    // no-sliding expiration.

        var afterExpFoo = Get(accessorFoo, DateTime.Now, (a, n, r) =>
            output.WriteLine($"after:Foo  {a:HH:mm:ss.fffZ}, next:{n:HH:mm:ss.fffZ}, {r} is {r >= TimeSpan.FromSeconds(1)}")
        );
        var afterExpBar = Get(accessorBar, DateTime.Now, (a, n, r) =>
            output.WriteLine($"after:Bar  {a:HH:mm:ss.fffZ}, next:{n:HH:mm:ss.fffZ}, {r} is {r >= TimeSpan.FromSeconds(1)}")
        );
        var afterExpBaz = Get(accessorBaz, DateTime.UtcNow, (a, n, _) =>
            output.WriteLine($"after:Baz  {a:HH:mm:ss.fffZ}")
        );

        // Assert
        //beforeFoo.IsNull();
        beforeBar.Is(data);
        beforeBaz.Is(data);

        afterExpFoo.Is(beforeExpFoo);
        afterExpBar.Is((value) => beforeExpBar < value);
        afterExpBaz.Is(beforeExpBaz);

        afterFoo.IsNull();
        afterBar.Is(data);
        afterBar.Is(data);

        return;
    }

}
