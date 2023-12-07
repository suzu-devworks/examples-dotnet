namespace Examples.Collections.Compatibility;

/// <summary>
/// Extension methods for <see cref="IEnumerable{T}" />.
/// </summary>
/// <seealso href="https://learn.microsoft.com/ja-jp/dotnet/core/whats-new/dotnet-6#new-linq-apis" />
public static class EnumerableExtensions
{
    /// <summary>
    /// Splits the elements of a sequence into chunks of size at most size.
    /// </summary>
    /// <remarks>
    /// <para>
    ///  Implemented as a heap practice with reference to the .NET 6.0 API.
    /// </para>
    /// </remarks>
    /// <param name="source"></param>
    /// <param name="chunkSize"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static IEnumerable<IEnumerable<T>> Chunk<T>(this IEnumerable<T> source, int chunkSize)
    {
        if (source == null) throw new ArgumentNullException(nameof(source));
        if (chunkSize < 0) throw new ArgumentOutOfRangeException($"{nameof(chunkSize)} is negative value `{chunkSize}`");

        return _();

        IEnumerable<IEnumerable<T>> _()
        {
            var sum = 0;
            var current = new LinkedList<T>();

            foreach (var item in source)
            {
                if ((sum > 0) && (chunkSize <= sum))
                {
                    yield return current;
                    current = new LinkedList<T>();
                    sum = 0;
                }

                current.AddLast(item);
                sum++;
            }

            if (sum > 0)
            {
                yield return current;
            }
        }
    }

    /// <summary>
    /// Returns distinct elements from a sequence according to a specified key selector function and using a specified comparer to compare keys.
    /// </summary>
    /// <remarks>
    /// <para>
    ///  Implemented as a heap practice with reference to the .NET 6.0 API.
    /// </para>
    /// </remarks>
    /// <param name="source"></param>
    /// <param name="keySelector"></param>
    /// <param name="comparer"></param>
    /// <typeparam name="TSource"></typeparam>
    /// <typeparam name="TKey"></typeparam>
    /// <returns></returns>
    public static IEnumerable<TSource> DistinctBy<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector, IEqualityComparer<TKey>? comparer = null)
    {
        if (source == null) throw new ArgumentNullException(nameof(source));
        if (keySelector == null) throw new ArgumentNullException(nameof(keySelector));

        return _();

        IEnumerable<TSource> _()
        {
            var keys = new HashSet<TKey>(comparer);
            foreach (var element in source)
            {
                var key = keySelector(element);
                if (keys.Add(key))
                {
                    yield return element;
                }
            }
        }
    }

    /// <summary>
    /// Splits the elements of a sequence into chunks of size at most size.
    /// </summary>
    /// <param name="source"></param>
    /// <param name="chunkSize"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    [Obsolete("Loops many times and is inefficient.")]
    public static IEnumerable<IEnumerable<T>> ChunkWithLinq<T>(this IEnumerable<T> source, int chunkSize)
    {
        if (chunkSize < 0) throw new ArgumentException($"{nameof(chunkSize)} is {chunkSize}");

        var current = source;
        while (current.Any())
        {
            yield return current.Take(chunkSize);
            current = current.Skip(chunkSize);
        }
    }

}
