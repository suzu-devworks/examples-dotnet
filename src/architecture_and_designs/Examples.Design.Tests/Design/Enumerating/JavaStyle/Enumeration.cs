using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Examples.Design.Enumerating.JavaStyle;

/// <summary>
/// Enumeration class like Java <c>Enum&lt;E extends Enum&lt;E&gt;&gt;</c>.
/// </summary>
/// <typeparam name="E"></typeparam>
/// <seealso Href="https://github.com/AdoptOpenJDK/openjdk-jdk11/blob/master/src/java.base/share/classes/java/lang/Enum.java"/>
public abstract class Enumeration<E> : IComparable<E>
    where E : Enumeration<E>
{
    protected Enumeration(string name, int ordinal)
    {
        this.Name = name;
        this.Ordinal = ordinal;
    }

    public string Name { get; }
    public int Ordinal { get; }

    public override int GetHashCode() => HashCode.Combine(this.Name);

    public override bool Equals(object? obj) => base.Equals(obj);

    public override string ToString() => this.Name;

    public int CompareTo(E? other)
    {
        return this.Name.CompareTo(other?.Name);
    }

    public static E? ValueOf(string name, bool ignoreCase = false)
    {
        return GetEnumConstants().FirstOrDefault(x => string.Compare(x.Name, name, ignoreCase) == 0);
    }

    public static IEnumerable<E> GetEnumConstants()
    {
        return typeof(E).GetFields(BindingFlags.Public |
                               BindingFlags.Static |
                               BindingFlags.DeclaredOnly)
                    .Select(f => f.GetValue(null))
                    .Cast<E>();

    }

}
