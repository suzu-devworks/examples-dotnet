using System.Reflection;

namespace Examples.Xunit;

/// <summary>
/// Extension methods for unit tests non public accessor.
/// </summary>
public static class NonPublicExtensions
{
    /// <summary>
    /// Invokes the non public method represented by the current instance, using  the specified parameters.
    /// </summary>
    /// <param name="instance">The object on which to invoke the method. </param>
    /// <param name="name">The string containing the name of the method to get.</param>
    /// <param name="args">The argument list for the invoked method. </param>
    /// <typeparam name="T">The type of instance.</typeparam>
    /// <typeparam name="TResult">The type of return value.</typeparam>
    /// <returns>An object containing the return value of the invoked method.</returns>
    /// <returns></returns>
    public static TResult? InvokeAs<T, TResult>(this T instance, string name, params object?[]? args)
            where T : notnull
            => (TResult?)instance.InvokeAs(typeof(T), name, args);

    /// <summary>
    /// Invokes the non public method represented by the current instance, using  the specified parameters.
    /// </summary>
    /// <param name="instance">The object on which to invoke the method. </param>
    /// <param name="name">The string containing the name of the method to get.</param>
    /// <param name="args">The argument list for the invoked method. </param>
    /// <typeparam name="T">The type of instance.</typeparam>
    /// <returns>An object containing the return value of the invoked method.</returns>
    public static object? InvokeAs<T>(this T instance, string name, params object?[]? args)
        where T : notnull
        => instance.InvokeAs(typeof(T), name, args);

    /// <summary>
    /// Invokes the non public method represented by the current instance, using  the specified parameters.
    /// </summary>
    /// <param name="instance">The object on which to invoke the method. </param>
    /// <param name="type">The type of instance.</param>
    /// <param name="name">The string containing the name of the method to get.</param>
    /// <param name="args">The argument list for the invoked method. </param>
    /// <returns>An object containing the return value of the invoked method.</returns>
    public static object? InvokeAs(this object instance, Type type, string name, params object?[]? args)
    {
        var method = type.GetMethod(name, BindingFlags.Instance | BindingFlags.NonPublic)
                    ?? throw new ArgumentException($"{name} is not found.");
        var result = method.Invoke(instance, args);

        return result;
    }

    /// <summary>
    /// Gets the non public property value of a specified object.
    /// </summary>
    /// <param name="instance">The object on which to invoke the method. </param>
    /// <param name="name">The string containing the name of the method to get.</param>
    /// <typeparam name="T">The type of instance.</typeparam>
    /// <typeparam name="TResult">The type of return value.</typeparam>
    /// <returns>The property value of the specified object.</returns>
    public static TResult? GetPropertyValueAs<T, TResult>(this T instance, string name)
        where T : notnull
        => (TResult?)instance.GetPropertyValueAs(typeof(T), name);

    /// <summary>
    /// Gets the non public property value of a specified object.
    /// </summary>
    /// <param name="instance">The object on which to invoke the method. </param>
    /// <param name="type">The type of instance.</param>
    /// <param name="name">The string containing the name of the method to get.</param>
    /// <returns>The property value of the specified object.</returns>
    public static object? GetPropertyValueAs(this object instance, Type type, string name)
        => GetPropertyAs(type, name).GetValue(instance);

    /// <summary>
    /// Sets the non public property value of a specified object.
    /// </summary>
    /// <param name="instance">The object on which to invoke the method. </param>
    /// <param name="name">The string containing the name of the method to get.</param>
    /// <param name="value">The new property value.</param>
    /// <typeparam name="T">The type of instance.</typeparam>
    /// <typeparam name="TValue">The type of store value.</typeparam>
    public static void SetPropertyValueAs<T, TValue>(this T instance, string name, TValue? value)
        where T : notnull
        => instance.SetPropertyValueAs(typeof(T), name, value);

    /// <summary>
    /// Sets the non public property value of a specified object.
    /// </summary>
    /// <param name="instance">The object on which to invoke the method. </param>
    /// <param name="type">The type of instance.</param>
    /// <param name="name">The string containing the name of the method to get.</param>
    /// <param name="value">The new property value.</param>
    public static void SetPropertyValueAs(this object instance, Type type, string name, object? value)
        => GetPropertyAs(type, name).SetValue(instance, value);

    private static PropertyInfo GetPropertyAs(Type type, string name)
        => type.GetProperty(name, BindingFlags.Instance | BindingFlags.NonPublic)
            ?? throw new ArgumentException($"{name} is not found.");

    /// <summary>
    /// Gets the non public field value of a specified object.
    /// </summary>
    /// <param name="instance">The object on which to invoke the method. </param>
    /// <param name="name">The string containing the name of the method to get.</param>
    /// <typeparam name="T">The type of instance.</typeparam>
    /// <typeparam name="TResult">The type of return value.</typeparam>
    /// <returns>The property value of the specified object.</returns>
    public static TResult? GetFieldValueAs<T, TResult>(this T instance, string name)
        where T : notnull
        => (TResult?)instance.GetFieldValueAs(typeof(T), name);

    /// <summary>
    /// Gets the non public field value of a specified object.
    /// </summary>
    /// <param name="instance">The object on which to invoke the method. </param>
    /// <param name="type">The type of instance.</param>
    /// <param name="name">The string containing the name of the method to get.</param>
    /// <returns>The property value of the specified object.</returns>
    public static object? GetFieldValueAs(this object instance, Type type, string name)
        => GetFieldAs(type, name).GetValue(instance);

    /// <summary>
    /// Sets the non public field value of a specified object.
    /// </summary>
    /// <param name="instance">The object on which to invoke the method. </param>
    /// <param name="name">The string containing the name of the method to get.</param>
    /// <param name="value">The new property value.</param>
    /// <typeparam name="T">The type of instance.</typeparam>
    /// <typeparam name="TValue">The type of store value.</typeparam>
    public static void SetFieldValueAs<T, TValue>(this T instance, string name, TValue? value)
        where T : notnull
        => instance.SetFieldValueAs(typeof(T), name, value);

    /// <summary>
    /// Sets the non public field value of a specified object.
    /// </summary>
    /// <param name="instance">The object on which to invoke the method. </param>
    /// <param name="type">The type of instance.</param>
    /// <param name="name">The string containing the name of the method to get.</param>
    /// <param name="value">The new property value.</param>
    public static void SetFieldValueAs(this object instance, Type type, string name, object? value)
        => GetFieldAs(type, name).SetValue(instance, value);

    private static FieldInfo GetFieldAs(Type type, string name)
        => type.GetField(name, BindingFlags.Instance | BindingFlags.NonPublic)
            ?? throw new ArgumentException($"{name} is not found.");

}
