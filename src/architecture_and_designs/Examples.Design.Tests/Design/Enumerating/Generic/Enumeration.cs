using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Examples.Design.Enumerating.Generic;

public abstract class Enumeration<T>
    where T : Enumeration<T>
{
    protected Enumeration(string? value, string? display)
        => (Value, Display) = (value, display);

    public string? Value { get; }
    public string? Display { get; }

    public virtual bool IsMatch(string value)
        => Value == value;

    public static IEnumerable<T> GetAll() =>
        typeof(T).GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.DeclaredOnly)
                 .Select(f => f.GetValue(null))
                 .Cast<T>();

    public static T? Parse(string? value)
    {
        if (value is null)
        {
            throw new ArgumentNullException(nameof(value));
        }

        var entry = GetAll().FirstOrDefault(x => x.IsMatch(value));
        var parsed = entry ?? default;

        return parsed;
    }

}
