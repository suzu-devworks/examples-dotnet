using System.Reflection;

namespace Examples.Reflection;

/// <summary>
/// Extension methods for non public accessor.
/// </summary>
public static class NonPublicReflectionExtensions
{

    public static object? InvokeNonPublic<T>(this T instance, string name, params object?[]? args)
        where T : notnull
        => instance.InvokeNonPublic(typeof(T), name, args);

    public static object? InvokeNonPublic(this object? instance, Type type, string name, params object?[]? args)
    {
        var result = GetNonPublicMethod(type, name).Invoke(instance, args);

        return result;
    }

    private static MethodInfo GetNonPublicMethod(Type type, string name)
    {
        var info = type.GetMethod(name, BindingFlags.Instance | BindingFlags.NonPublic)
                    ?? throw new ArgumentException($"{name} is not found.");

        return info;
    }


    public static object? GetNonPublicPropertyValue<T>(this T instance, string name)
        where T : notnull
        => instance.GetNonPublicPropertyValue(typeof(T), name);

    public static object? GetNonPublicPropertyValue(this object? instance, Type type, string name)
    {
        var value = GetNonPublicProperty(type, name).GetValue(instance);

        return value;
    }

    public static void SetNonPublicPropertyValue<T>(this T instance, string name, object? value)
        where T : notnull
        => instance.SetNonPublicPropertyValue(typeof(T), name, value);

    public static void SetNonPublicPropertyValue(this object instance, Type type, string name, object? value)
    {
        GetNonPublicProperty(type, name).SetValue(instance, value);

        return;
    }

    private static PropertyInfo GetNonPublicProperty(Type type, string name)
    {
        return type.GetProperty(name, BindingFlags.Instance | BindingFlags.NonPublic)
                            ?? throw new ArgumentException($"{name} is not found.");
    }


    public static object? GetNonPublicFieldValue<T>(this T instance, string name)
        where T : notnull
        => instance.GetNonPublicFieldValue(typeof(T), name);

    public static object? GetNonPublicFieldValue(this object? instance, Type type, string name)
    {
        var value = GetNonPublicField(type, name).GetValue(instance);

        return value;
    }

    private static FieldInfo GetNonPublicField(Type type, string name)
    {
        return type.GetField(name, BindingFlags.Instance | BindingFlags.NonPublic)
                            ?? throw new ArgumentException($"{name} is not found.");
    }

}
