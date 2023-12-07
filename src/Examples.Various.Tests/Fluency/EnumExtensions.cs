using System.Collections.Concurrent;
using System.ComponentModel;
using System.Linq.Expressions;
using System.Reflection;

namespace Examples.Fluency;

/// <summary>
/// Extension methods for <c>enum</c>.
/// </summary>
public static class EnumExtensions
{
    /// <summary>
    /// Gets the <see cref="DescriptionAttribute.Description" /> string set in the definition value of the <c>enum</c>.
    /// </summary>
    /// <param name="value">The <c>enum</c> value.</param>
    /// <typeparam name="TEnum">The <c>enum</c> type.</typeparam>
    /// <returns>The <see cref="DescriptionAttribute.Description" /> string.</returns>
    public static string? GetDescription<TEnum>(this TEnum value)
        where TEnum : Enum
        => value.GetDescriptionWithDelegateCached();

    /// <summary>
    /// Gets the <see cref="DescriptionAttribute.Description" /> string set in the definition value of the <c>enum</c>
    ///  Use reflection.
    /// </summary>
    /// <param name="value">The <c>enum</c> value.</param>
    /// <returns>The <see cref="DescriptionAttribute.Description" /> string.</returns>
    public static string? GetDescriptionWithReflection(this Enum value)
    {
        // Enum.ToString() too late.
        var field = value.GetType().GetField(value.ToString());
        var attributes = field?.GetCustomAttributes<DescriptionAttribute>(false)
            as DescriptionAttribute[];
        var description = attributes?.FirstOrDefault()?.Description;


        return description ?? value.ToString();
    }

    /// <summary>
    /// Gets the <see cref="DescriptionAttribute.Description" /> string set in the definition value of the <c>enum</c>,
    /// Use the <see cref="Type" /> and <c>enum</c> keys to cache the Description.
    /// </summary>
    /// <param name="value">The <c>enum</c> value.</param>
    /// <returns>The <see cref="DescriptionAttribute.Description" /> string.</returns>
    public static string? GetDescriptionWithValueCached(this Enum value)
    {
        var key = (value.GetType(), value);

        if (!CachedValues.TryGetValue(key, out var name))
        {
            name = value.GetDescriptionWithReflection() ?? "";
            CachedValues[key] = name;
        }

        return name;
    }

    private static readonly ConcurrentDictionary<(Type, Enum), string> CachedValues = new();

    /// <summary>
    /// Gets the <see cref="DescriptionAttribute.Description" /> string set in the definition value of the <c>enum</c>
    /// Use the <see cref="Type" /> keys to cache the Switch-Case expression.
    /// </summary>
    /// <param name="value">The <c>enum</c> value.</param>
    /// <typeparam name="TEnum">The <c>enum</c> type.</typeparam>
    /// <returns>The <see cref="DescriptionAttribute.Description" /> string.</returns>
    public static string? GetDescriptionWithDelegateCached<TEnum>(this TEnum value)
        where TEnum : Enum
    {
        var key = value.GetType();

        if (!CachedCases.TryGetValue(key, out var func))
        {
            func = CreateDescriptionSelector<TEnum>();
            CachedCases[key] = func;
        }

        var selector = func as Func<TEnum, string>;
        var name = selector?.Invoke(value);

        return name;
    }

    private static readonly ConcurrentDictionary<Type, Delegate> CachedCases = new();

    private static Func<TEnum, string?> CreateDescriptionSelector<TEnum>()
        where TEnum : Enum
    {
        var paramType = typeof(TEnum);
        var parameter = Expression.Parameter(paramType, "param");

        var resultType = typeof(string);
        var result = Expression.Variable(resultType, "result");

        var cases = Enum.GetValues(paramType).OfType<TEnum>()
            .Select(x =>
                Expression.SwitchCase(
                    Expression.Assign(result,
                        Expression.Constant(x.GetDescriptionWithReflection() ?? "")),
                    Expression.Constant(x)));

        var body = Expression.Block(
            resultType,
            new[] { result },
            Expression.Assign(result, Expression.Constant(null, resultType)),
            Expression.Switch(
                parameter,
                Expression.Assign(result, Expression.Constant(null, resultType)),
                cases.ToArray()));

        var lambda = Expression.Lambda<Func<TEnum, string>>(body, parameter);
        var func = lambda.Compile();

        return func;
    }

}
