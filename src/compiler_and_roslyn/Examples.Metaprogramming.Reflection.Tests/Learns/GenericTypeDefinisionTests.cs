namespace Examples.Metaprogramming.Reflection.Tests.Learns;

/// <summary>
/// Tests that creates a generic type class using a <see cref="Type" /> object.
/// </summary>
public class GenericTypeDefinitionTests
{
    // closed generic.
    private readonly Type _expectedClosedGenericType = typeof(List<ValueTuple<string, int>>);

    [Fact]
    public void When_CreatingClosedGenericType_WithFullyQualifiedName_Then_ReturnsTheExpectedType()
    {
        var type = Type.GetType(
                "System.Collections.Generic.List`1["
                    + "[System.ValueTuple`2["
                        + "[System.String],"
                        + "[System.Int32]"
                    + "]]"
                + "]");

        Assert.Equal(_expectedClosedGenericType, type);
    }

    [Fact]
    public void When_CreatingClosedGenericType_WithAppendAssemblyName_Then_ReturnsTheExpectedType()
    {
        var type = Type.GetType(
                "System.Collections.Generic.List`1["
                    + "[System.ValueTuple`2["
                        + "[System.String, System.Private.CoreLib],"
                        + "[System.Int32, System.Private.CoreLib]"
                    + "], System.Private.CoreLib]"
                + "], System.Private.CoreLib");

        Assert.Equal(_expectedClosedGenericType, type);
    }

    [Fact]
    public void When_CreatingClosedGenericType_WithTypeInstances_Then_ReturnsTheExpectedType()
    {
        var typeOfList = Type.GetType("System.Collections.Generic.List`1");
        var typeOfValueTuple = Type.GetType("System.ValueTuple`2");
        var typeOfValueTupleSpecific
            = typeOfValueTuple?.MakeGenericType(
                Type.GetType("System.String")!,
                Type.GetType("System.Int32")!);

        var type = typeOfList?.MakeGenericType(typeOfValueTupleSpecific!);

        Assert.Equal(_expectedClosedGenericType, type);
    }

    [Fact]
    public void When_CreatingClosedGenericType_WithTypeof_Then_ReturnsTheExpectedType()
    {
        var typeOfList = typeof(List<>);
        var typeOfValueTuple = typeof(ValueTuple<,>);
        var typeOfValueTupleSpecific = typeOfValueTuple?.MakeGenericType(
                typeof(string),
                typeof(int));

        var type = typeOfList?.MakeGenericType(typeOfValueTupleSpecific!);

        Assert.Equal(_expectedClosedGenericType, type);
    }

    // open generic.
    private readonly Type _expectedOpenGenericType = typeof(ValueTuple<>);

    [Fact]
    public void When_CreatingOpenGenericType_WithFullyQualifiedName_Then_ReturnsTheExpectedType()
    {
        var type = Type.GetType("System.ValueTuple`1, System.Private.CoreLib");

        Assert.Equal(_expectedOpenGenericType, type);
    }

    // inner open generic.
    private readonly Type _expectedInnerGenericType = typeof(DummyGeneric<,,>);

    [Fact]
    public void When_CreatingInnerOpenGenericType_WithFullyQualifiedName_Then_ReturnsTheExpectedType()
    {
        var type = Type.GetType($"{GetType().FullName}+DummyGeneric`3, {GetType().Assembly.FullName}");

        Assert.Equal(_expectedInnerGenericType, type);
    }

    public class DummyGeneric<T1, T2, T3>
    {
    }

    // not exists generic.

    [Fact]
    public void When_CreatingNotExistsGenericType_Then_ReturnsNull()
    {
        var type = Type.GetType("Examples.Enumerations.Enumeration`1");

        Assert.Null(type);
    }
}
