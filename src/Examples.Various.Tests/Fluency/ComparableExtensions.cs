namespace Examples.Fluency;

/// <summary>
/// Extension methods for <see cref="IComparable{T}" />.
/// </summary>
public static class ComparableExtensions
{
    /// <summary>
    /// Gets a value that indicates whether it is included in the upper and lower limits of the range.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="target">The <see cref="IComparable{T}" /> object instance.</param>
    /// <param name="lower">An <see cref="IComparable{T}" /> object instance that indicates the lower limit of the range</param>
    /// <param name="higher">An <see cref="IComparable{T}" /> object instance that indicates the higher limit of the range</param>
    /// <returns> <c>true</c> if included in the specified range.
    ///     <c>false</c> if not in range.</returns>
    public static bool Between<T>(this IComparable<T> target, T? lower, T? higher)
    {
        return (0 <= target.CompareTo(lower)) && (target.CompareTo(higher) <= 0);
    }

}

