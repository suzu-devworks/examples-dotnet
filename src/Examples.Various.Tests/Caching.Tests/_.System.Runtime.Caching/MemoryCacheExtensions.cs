using Examples.Xunit;

namespace System.Runtime.Caching;

/// <summary>
/// Extension method for <see cref="MemoryCache" /> to spoofing <see cref="DateTime.Now" />.
/// </summary>
/// <remarks>
/// The <see cref="MemoryCache" /> uses <see cref="DateTime.Now" /> internally, so it's hard to test.
/// </remarks>
public static class MemoryCacheExtensions
{
    public static MemoryCacheEntryAccessor GetEntryAccessor(this MemoryCache cache, string key)
    {
        // https://github.com/dotnet/runtime/blob/main/src/libraries/System.Runtime.Caching/src/System/Runtime/Caching/MemoryCache.cs#L588
        // public override object this[string key]

        // https://github.com/dotnet/runtime/blob/main/src/libraries/System.Runtime.Caching/src/System/Runtime/Caching/MemoryCache.cs#L509
        // private object GetInternal(string key, string regionName)

        // https://github.com/dotnet/runtime/blob/main/src/libraries/System.Runtime.Caching/src/System/Runtime/Caching/MemoryCache.cs#L523
        // internal MemoryCacheEntry GetEntry(string key)

        var entry = cache.InvokeAs("GetEntry", key)
            ?? throw new InvalidOperationException($"`MemoryCache#GetEntry( {key} )` is null.");

        return new(entry);
    }

    /// <summary>
    /// MemoryCacheEntry internal instance accessor.
    /// </summary>
    public class MemoryCacheEntryAccessor(object entry)
    {

        /// <summary>
        ///
        /// </summary>
        /// <remarks>
        /// <seealso href="https://github.com/dotnet/runtime/blob/main/src/libraries/System.Runtime.Caching/src/System/Runtime/Caching/MemoryCacheEntry.cs#L50"/>
        /// <code>
        ///  internal DateTime UtcAbsExp { get; set; }
        /// </code>
        /// </remarks>
        public DateTime UtcAbsExp
        {
            get => (DateTime)entry.GetPropertyValueAs(entry.GetType(), "UtcAbsExp")!;
            set => entry.SetPropertyValueAs(entry.GetType(), "UtcAbsExp", value);
        }

        /// <summary>
        ///
        /// </summary>
        /// <remarks>
        /// <seealso href="https://github.com/dotnet/runtime/blob/main/src/libraries/System.Runtime.Caching/src/System/Runtime/Caching/MemoryCacheEntry.cs#L78"/>
        /// <code>
        ///  internal TimeSpan SlidingExp { get; }
        /// </code>
        /// </remarks>
        public TimeSpan SlidingExp
        {
            get => (TimeSpan)entry.GetPropertyValueAs(entry.GetType(), "SlidingExp")!;
            set => entry.SetFieldValueAs(entry.GetType(), "_slidingExp", value);
        }

    }

}
