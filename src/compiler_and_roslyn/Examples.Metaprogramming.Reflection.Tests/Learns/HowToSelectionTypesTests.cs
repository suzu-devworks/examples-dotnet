using System.Reflection;

namespace Examples.Metaprogramming.Reflection.Tests.Learns;

public class HowToSelectionTypesTests
{
    private static ITestOutputHelper? Output => TestContext.Current.TestOutputHelper;

    [Fact]
    public void When_GetTypes_WithSpecifiedInterface_Then_ReturnsExpectedTypes()
    {
        // Given
        Assembly assembly = typeof(string).Assembly;
        Type targetInterface = typeof(System.Collections.IEnumerable);

        // is not generic type
        Assert.False(targetInterface.IsGenericType);

        // When
        var types = assembly.GetTypes()
             .Where(type => type.GetInterfaces().Contains(targetInterface))
             .ToList();

        Output?.WriteLine($"Found {types.Count} types implementing {targetInterface.FullName}.");

        // Then
        Assert.NotEmpty(types);
        Assert.Contains(types.First().GetInterfaces(), i => i == targetInterface);
    }

    [Fact]
    public void When_GetTypes_WithOpenGenericInterface_Then_ReturnsExpectedTypes()
    {
        // Given
        Assembly assembly = typeof(string).Assembly;
        Type targetInterface = typeof(System.Collections.Generic.IEnumerable<>);

        // is open generic type
        Assert.True(targetInterface.IsGenericType && targetInterface.ContainsGenericParameters);

        // When
        var types = assembly.GetTypes()
            .Where(type => type.GetInterfaces()
                .Any(i => i.IsConstructedGenericType && i.GetGenericTypeDefinition() == targetInterface)
            )
            .ToList();

        Output?.WriteLine($"Found {types.Count} types implementing {targetInterface.FullName}.");

        // Then
        Assert.NotEmpty(types);
        Assert.Contains(types.First().GetInterfaces()
            , i => i.IsConstructedGenericType && i.GetGenericTypeDefinition() == targetInterface
        );
    }

    [Fact]
    public void When_GetTypes_WithClosedGenericInterface_Then_ReturnsExpectedTypes()
    {
        // Given
        Assembly assembly = typeof(string).Assembly;
        Type targetInterface = typeof(System.Collections.Generic.IEnumerable<char>);

        // is closed generic type
        Assert.True(targetInterface.IsGenericType && !targetInterface.ContainsGenericParameters);

        // When
        var types = assembly.GetTypes()
            .Where(type => type.GetInterfaces().Contains(targetInterface))
            .ToList();

        Output?.WriteLine($"Found {types.Count} types implementing {targetInterface.FullName}.");

        // Then
        Assert.NotEmpty(types);

        Assert.Contains(types.First().GetInterfaces()
            , i => i.IsConstructedGenericType
                && i.GetGenericTypeDefinition() == typeof(System.Collections.Generic.IEnumerable<>)
        );
        Assert.Contains(types.First().GetInterfaces(), i => i == targetInterface);
    }

    [Fact]
    public void When_GetTypes_WithSpecifiedAttribute_Then_ReturnsExpectedTypes()
    {
        // Given
        Assembly assembly = typeof(string).Assembly;
        Type targetAttribute = typeof(System.SerializableAttribute);

        // When
        var types = assembly.GetTypes()
             .Where(t => t.IsDefined(targetAttribute, inherit: false))
             .ToList();

        Output?.WriteLine($"Found {types.Count} types with {targetAttribute.FullName}.");

        // Then
        Assert.NotEmpty(types);
        Assert.Contains(types.First().GetCustomAttributes(inherit: false)
            , attr => attr.GetType() == targetAttribute);
    }

    [Fact]
    public void When_GetTypes_WithSpecifiedBaseType_Then_ReturnsExpectedTypes()
    {
        // Given
        Assembly assembly = typeof(string).Assembly;
        Type targetBaseType = typeof(System.Exception);

        // When
        var types = assembly.GetTypes()
             //  .Where(t => t.BaseType == targetBaseType)  // When Direct inheritance only.
             .Where(t => t.IsSubclassOf(targetBaseType))    // When Including descendants.
             .ToList();

        Output?.WriteLine($"Found {types.Count} types extend {targetBaseType.FullName}.");

        // Then
        Assert.NotEmpty(types);
        Assert.True(types.First().IsSubclassOf(targetBaseType));
    }

    [Fact]
    public void When_GetTypes_WithSpecifiedProperty_Then_ReturnsExpectedTypes()
    {
        // Given
        Assembly assembly = typeof(string).Assembly;
        string targetPropertyName = "Length";

        // When
        var types = assembly.GetTypes()
             .Where(t => t.GetProperties().Any(p => p.Name == targetPropertyName))
             .ToList();

        Output?.WriteLine($"Found {types.Count} types has {targetPropertyName}.");

        // Then
        Assert.NotEmpty(types);
        Assert.Contains(types.First().GetProperties(), p => p.Name == targetPropertyName);
    }

    [Fact]
    public void When_GetTypes_WithSpecifiedMethodNameAndArgs_Then_ReturnsExpectedTypes()
    {
        // Given
        Assembly assembly = typeof(string).Assembly;
        string targetMethodName = "IndexOf";
        Type[] targetMethodArgs = [typeof(char)];

        // When
        var types = assembly.GetTypes()
             .Where(t => t.GetMethods().Any(m =>
                 m.Name == targetMethodName &&
                 m.GetParameters().Select(p => p.ParameterType).SequenceEqual(targetMethodArgs)
             ))
             .ToList();

        Output?.WriteLine($"Found {types.Count} types has {targetMethodName}({string.Join(",", targetMethodArgs)}).");

        // Then
        Assert.NotEmpty(types);
        Assert.Contains(types.First().GetMethods(), m =>
            m.Name == targetMethodName &&
            m.GetParameters().Select(p => p.ParameterType).SequenceEqual(targetMethodArgs)
        );
    }
}
