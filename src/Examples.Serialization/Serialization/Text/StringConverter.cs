using System.Text.RegularExpressions;

namespace Examples.Serialization.Text;

/// <summary>
/// Converts a <see cref="string"/> to or from some types.
/// </summary>
public static partial class StringConverter
{
    /// <summary>
    /// Converts the specified string to an <see cref="Index" /> value.
    /// </summary>
    /// <param name="value">The string representation of an index.</param>
    /// <returns>An <see cref="Index" /> value.</returns>
    /// <exception cref="ArgumentNullException">Thrown if input is null.</exception>
    /// <exception cref="ArgumentException">Thrown if input is illegal value.</exception>
    public static Index ToIndex(string? value)
    {
        ArgumentNullException.ThrowIfNull(value);

        var match = IndexMatcher.Match(value);
        if (!match.Success)
        {
            throw new ArgumentException($"Illegal value is [{value}].");
        }

        var fromEnd = match.Groups[1].Value == "^";
        var converted = int.Parse(match.Groups[2].Value);

        return new(converted, fromEnd);
    }

#if NET7_0_OR_GREATER
    private static readonly Regex IndexMatcher = CreateIndexMatcher();

    [GeneratedRegex(@"^(\^?)(\d{1,9})$", RegexOptions.Compiled)]
    private static partial Regex CreateIndexMatcher();

#else
    private static readonly Regex IndexMatcher = new(@"^(\^?)(\d{1,9})$", RegexOptions.Compiled);

#endif


    /// <summary>
    /// Converts the specified string to a <see cref="Range" /> value.
    /// </summary>
    /// <param name="value">The string representation of a range.</param>
    /// <returns>A <see cref="Range" /> value.</returns>
    /// <exception cref="ArgumentNullException">Thrown if input is null.</exception>
    /// <exception cref="ArgumentException">Thrown if input is illegal value.</exception>
    public static Range ToRange(string? value)
    {
        ArgumentNullException.ThrowIfNull(value);

        var indexes = value.Split("..");
        if (indexes is not { Length: 2 })
        {
            throw new ArgumentException($"Illegal value is [{value}].");
        }

        var start = ToIndex(indexes[0]);
        var end = ToIndex(indexes[1]);

        return new(start, end);
    }

}
