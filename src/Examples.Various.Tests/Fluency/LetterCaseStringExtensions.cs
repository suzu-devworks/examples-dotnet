using System.Text.RegularExpressions;

namespace Examples.Fluency;

/// <summary>
/// Extension methods for <see cref="string" /> to convert letter case.
/// </summary>
public static partial class LetterCaseStringExtensions
{
    /// <summary>
    /// Returns a copy of this string converted to snake case.
    /// </summary>
    /// <param name="input">The string to convert to snake case.</param>
    /// <returns>A string in snake case.</returns>
    public static string ToSnakeCase(this string input)
    {
        return CamelCaseMatcher.Replace(input, match =>
            $"{match.Groups[0].Value.AsSpan()[0..1].ToString()}_{match.Groups[0].Value.AsSpan()[1..2].ToString()}").ToLower();
    }

#if NET7_0_OR_GREATER
    private static readonly Regex CamelCaseMatcher = CreateCamelCaseMatcher();

    [GeneratedRegex(@"[a-z][A-Z]", RegexOptions.Compiled)]
    private static partial Regex CreateCamelCaseMatcher();

#else
    private static readonly Regex CamelCaseMatcher = new(@"[a-z][A-Z]", RegexOptions.Compiled);

#endif


    /// <summary>
    /// Returns a copy of this string converted to lower camel case.
    /// </summary>
    /// <param name="input">The string to convert to camel case.</param>
    /// <returns>A string in camel case.</returns>
    public static string ToCamelCase(this string input)
    {
        return SnakeCaseMatcher.Replace(input, match =>
            $"{match.Groups[0].Value.AsSpan()[0..1].ToString()}{match.Groups[0].Value.AsSpan()[2..3].ToString().ToUpper()}");
    }

#if NET7_0_OR_GREATER
    private static readonly Regex SnakeCaseMatcher = CreateSnakeCaseMatcher();

    [GeneratedRegex(@"\w_\w", RegexOptions.Compiled)]
    private static partial Regex CreateSnakeCaseMatcher();

#else
    private static readonly Regex SnakeCaseMatcher = new(@"\w_\w", RegexOptions.Compiled);

#endif

}
