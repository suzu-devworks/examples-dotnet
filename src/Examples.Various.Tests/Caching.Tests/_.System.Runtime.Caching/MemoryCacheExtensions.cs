using System.Collections;
using System.Reflection;
using Examples.Reflection;

namespace System.Runtime.Caching;

/// <summary>
/// Extension method for <see href="MemoryCache" /> to spoofing <see cref="DateTime.Now" />.
/// </summary>
/// <remarks>
/// <para><see cref="MemoryCache" /> uses <see cref="DateTime.Now" /> internally, so it's hard to test.</para>
/// </remarks>
/// <seealso href="https://github.com/dotnet/runtime/blob/main/src/libraries/System.Runtime.Caching/src/System/Runtime/Caching/MemoryCacheStore.cs#L295" />
/// <seealso href="https://github.com/dotnet/runtime/blob/main/src/libraries/System.Runtime.Caching/src/System/Runtime/Caching/MemoryCacheEntry.cs#L309" />
/// <seealso href="https://github.com/dotnet/runtime/blob/main/src/libraries/System.Runtime.Caching/src/System/Runtime/Caching/CacheExpires.cs#L760" />
public static class RuntimeMemoryCacheExtensions
{
    public static void SetAbsoluteExpiration(this MemoryCache cache, string key, DateTime limit)
    {
        // Get internal class MemoryCacheEntry instance.
        var entry = cache.GetEntry(key)
            ?? throw new InvalidOperationException($"GetEntry({key}) is null.");

        // Set value to internal UtcAbsExp property.
        entry.SetNonPublicPropertyValue(entry.GetType(), "UtcAbsExp", limit);

        return;
    }

    public static DateTime? GetAbsoluteExpiration(this MemoryCache cache, string key)
    {
        // Get internal class MemoryCacheEntry instance.
        var entry = cache.GetEntry(key)
            ?? throw new InvalidOperationException($"GetEntry({key}) is null.");

        // Get value to internal UtcAbsExp property.
        var value = entry.GetNonPublicPropertyValue(entry.GetType(), "UtcAbsExp");

        return (DateTime?)value;
    }

    private static object? GetEntry(this MemoryCache cache, string key)
    {
        // Calling GetEntry will perform expired updates.
        // var entry = cache.InvokeNonPublic("GetEntry", key)
        //     ?? throw new InvalidOperationException($"GetEntry({key}) is null.");

        var keyType = typeof(MemoryCache).Assembly.GetType("System.Runtime.Caching.MemoryCacheKey")
            ?? throw new InvalidOperationException("`System.Runtime.Caching.MemoryCacheKey` not found.");

        var keyInstance = Activator.CreateInstance(
            keyType,
            BindingFlags.Instance | BindingFlags.NonPublic,
            null,
            [key],
            null)
            ?? throw new InvalidOperationException($"`System.Runtime.Caching.MemoryCacheKey` is null.");

        var store = cache.InvokeNonPublic("GetStore", keyInstance)
            ?? throw new InvalidOperationException($"`GetStore( {key} )` is null.");

        var entries = (Hashtable?)store.GetNonPublicFieldValue(store.GetType(), "_entries");

        var entry = entries?[keyInstance];

        return entry;
    }

}
