using System.ComponentModel;
using System.Globalization;

namespace Examples.Fluency;

public static partial class TypeConvertorStringExtensions
{
    public static T? ParseTo<T>(this string input, CultureInfo? culture = null)
        => input.TryParseTo<T>(culture, out T? value) ? value : default;

    public static bool TryParseTo<T>(this string input, out T? value)
        => input.TryParseTo(null, out value);

    public static bool TryParseTo<T>(this string input, CultureInfo? culture, out T? value)
    {
        var success = input.TryParseTo(typeof(T), culture, out var result);
        value = success ? (T?)result : default;
        return success;
    }

    public static object? ParseTo(this string input, Type targetType)
    {
        return input.TryParseTo(targetType, null, out var value) ? value : default;
    }

    public static object? ParseTo(this string input, Type targetType, CultureInfo? culture = null)
        => input.TryParseTo(targetType, culture, out var value) ? value : default;

    public static bool TryParseTo(this string input, Type targetType, out object? value)
        => input.TryParseTo(targetType, null, out value);

    public static bool TryParseTo(this string input, Type targetType, CultureInfo? culture, out object? value)
    {
        ArgumentNullException.ThrowIfNull(targetType);

        var underlyingType = Nullable.GetUnderlyingType(targetType);
        var actualTargetType = underlyingType ?? targetType;

        if (input is null)
        {
            value = default;
            return false;
        }

        if (input.Length == 0)
        {
            if (actualTargetType == typeof(string))
            {
                value = string.Empty;
                return true;
            }

            if (underlyingType is not null)
            {
                value = null;
                return true;
            }

            value = default;
            return false;
        }

        culture ??= CultureInfo.InvariantCulture;

        var convertor = TypeDescriptor.GetConverter(actualTargetType);
        if (convertor is null || !convertor.CanConvertFrom(typeof(string)))
        {
            value = default;
            return false;
        }

        try
        {
            value = convertor.ConvertFromString(null, culture, input);

            if (value is null && underlyingType is null && actualTargetType.IsValueType)
            {
                return false;
            }

            return true;
        }
        catch (Exception e) when (e is ArgumentException
                                    or FormatException
                                    or InvalidCastException
                                    or NotSupportedException)
        {
            value = default;
            return false;
        }
    }
}
