using System.Globalization;
using System.Reflection;

namespace Examples.Metaprogramming.Reflection.Tests.Learns;

/// <summary>
/// Tests that demonstrates how to use reflection to retrieve type information at runtime.
/// </summary>
public class RuntimeTypesTests
{
    [Fact]
    public void When_UsingTypeofOnType_Then_CanRetrieveTypeInformation()
    {
        var type = typeof(string);
        Assert.Equal("System.String", type.FullName);
        Assert.Equal("String", type.Name);
        Assert.Equal("System", type.Namespace);
        Assert.StartsWith("System.String, System.Private.CoreLib,", type.AssemblyQualifiedName);
        Assert.True(type.IsClass);
        Assert.False(type.IsInterface);
        Assert.False(type.IsValueType);
    }

    [Fact]
    public void When_UsingAssembly_Then_CanRetrieveAssemblyInformation()
    {
        var type = typeof(string);
        var assembly = type.Assembly;

        Assert.StartsWith("System.Private.CoreLib, ", assembly.FullName);
        Assert.True(Path.Exists(assembly.Location));

        var assemblyName = assembly.GetName();
        Assert.Equal("System.Private.CoreLib", assemblyName.Name);
        Assert.True(assemblyName.Version?.Major >= 10);
        Assert.Equal(CultureInfo.InvariantCulture, assemblyName.CultureInfo);
    }

    [Fact]
    public void When_UsingModule_Then_CanRetrieveModuleInformation()
    {
        var type = typeof(string);
        var module = type.Module;
        Assert.Equal("System.Private.CoreLib.dll", module.Name);
        Assert.Equal("System.Private.CoreLib.dll", module.ScopeName);
        Assert.True(Path.Exists(module.FullyQualifiedName));
    }

    [Fact]
    public void When_UsingConstructorInfo_Then_CanRetrieveConstructorInformation()
    {
        var type = typeof(string);
        var constructors = type.GetConstructors();
        Assert.NotEmpty(constructors);

        var constructorInfo = type.GetConstructor([typeof(char[])]);
        Assert.NotNull(constructorInfo);
        Assert.Equal(".ctor", constructorInfo.Name);
        Assert.Equal(type, constructorInfo.DeclaringType);
        Assert.Single(constructorInfo.GetParameters());

        var result = constructorInfo.Invoke(["Hello".ToCharArray()]);
        Assert.IsType<string>(result);
        Assert.Equal("Hello", result);
    }

    [Fact]
    public void When_UsingMethodInfo_Then_CanRetrieveMethodInformation()
    {
        var type = typeof(string);
        var methods = type.GetMethods();
        Assert.NotEmpty(methods);

        var methodInfo = type.GetMethod("Contains", [typeof(string)]);
        Assert.NotNull(methodInfo);
        Assert.Equal("Contains", methodInfo.Name);
        Assert.Equal(type, methodInfo.DeclaringType);
        Assert.Single(methodInfo.GetParameters());
        Assert.Equal(typeof(bool), methodInfo.ReturnType);

        var result = methodInfo.Invoke("Hello, World!", ["World"]);
        Assert.IsType<bool>(result);
        Assert.True((bool)result);
    }

    [Fact]
    public void When_UsingFieldInfo_Then_CanRetrievePropertyInformation()
    {
        var type = typeof(string);
        var fields = type.GetFields();
        Assert.NotEmpty(fields);

        var fieldInfo = type.GetField("Empty", BindingFlags.Public | BindingFlags.Static);
        Assert.NotNull(fieldInfo);
        Assert.Equal("Empty", fieldInfo.Name);
        Assert.Equal(type, fieldInfo.DeclaringType);
        Assert.Equal(typeof(string), fieldInfo.FieldType);

        var result = fieldInfo.GetValue(null);
        Assert.IsType<string>(result);
        Assert.Equal(string.Empty, result);
    }

    [Fact]
    public void When_UsingEventInfo_Then_CanRetrieveEventInformation()
    {
        var type = typeof(string);
        var events = type.GetEvents();
        Assert.Empty(events);

        var type2 = typeof(System.Timers.Timer);
        var events2 = type2.GetEvents();
        Assert.NotEmpty(events2);

        var eventInfo = type2.GetEvent("Elapsed");
        Assert.NotNull(eventInfo);
        Assert.Equal("Elapsed", eventInfo.Name);
        Assert.Equal(type2, eventInfo.DeclaringType);
        Assert.Equal(typeof(System.Timers.ElapsedEventHandler), eventInfo.EventHandlerType);
    }

    [Fact]
    public void When_UsingPropertyInfo_Then_CanRetrievePropertyInformation()
    {
        var type = typeof(string);
        var properties = type.GetProperties();
        Assert.NotEmpty(properties);

        var propertyInfo = type.GetProperty("Length");
        Assert.NotNull(propertyInfo);
        Assert.Equal("Length", propertyInfo.Name);
        Assert.Equal(type, propertyInfo.DeclaringType);
        Assert.Equal(typeof(int), propertyInfo.PropertyType);

        var result = propertyInfo.GetValue("Hello");
        Assert.IsType<int>(result);
        Assert.Equal(5, result);
    }

    [Fact]
    public void When_UsingCustomAttribute_Then_CanRetrieveAttributeInformation()
    {
        var type = typeof(FactAttribute);
        var attributes = type.GetCustomAttributes();
        Assert.NotEmpty(attributes);

        var attributeInfo = type.GetCustomAttribute<AttributeUsageAttribute>();
        Assert.NotNull(attributeInfo);
        Assert.Equal("System.AttributeUsageAttribute", attributeInfo.GetType().FullName);
    }
}
