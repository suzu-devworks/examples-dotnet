using System.ComponentModel;
using System.Globalization;
using System.Text.RegularExpressions;

namespace Examples.Fluency;

public static class StringExtensions
{
    public static string ReplaceWithRegex(this string source, string pattern, string replacement, RegexOptions options = default)
    {
        ArgumentNullException.ThrowIfNull(pattern);
        ArgumentNullException.ThrowIfNull(replacement);

        return Regex.Replace(source, pattern, replacement, options);
    }

    public static bool In(this string source, params string?[] candidate)
    {
        ArgumentNullException.ThrowIfNull(candidate);

        if (candidate.Length == 0)
        {
            return false;
        }

        return candidate.Contains(source);
    }

    public static (T? Value, bool Success) ParseTo<T>(this string input, CultureInfo? culture = null)
    {
        var (value, success) = ParseTo(input, typeof(T), culture);
        return ((T?)value, success);
    }

    public static (object? Value, bool Success) ParseTo(this string input, Type targetType, CultureInfo? culture = null)
    {
        if (string.IsNullOrEmpty(input))
        {
            return (targetType.GetTypeDefault(), false);
        }

        var convertor = TypeDescriptor.GetConverter(targetType);
        if (convertor is null)
        {
            return (targetType.GetTypeDefault(), false);
        }

        try
        {
            var result = convertor.ConvertFromString(null, culture, input);
            return (result, true);
        }
        catch (ArgumentException e) when (e.InnerException is FormatException)
        {
            return (targetType.GetTypeDefault(), false);
        }
    }

    private static object? GetTypeDefault(this Type targetType)
        => (targetType.IsValueType && (Nullable.GetUnderlyingType(targetType) is null))
            ? Activator.CreateInstance(targetType)
            : null;

    public static T? With<T>(this string source, Func<string, T?> convertor)
        => convertor.Invoke(source);

}
