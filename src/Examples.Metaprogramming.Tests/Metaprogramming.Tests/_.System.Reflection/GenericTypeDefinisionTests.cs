namespace Examples.Metaprogramming.Tests._.System.Reflection;

/// <summary>
/// Tests that creates a generic type class using a <see cref="Type" /> object.
/// </summary>
public class GenericTypeDefinitionTests
{
    // closed generic.
    private readonly Type _expectedClosedGenericType = typeof(List<ValueTuple<string, int>>);

    [Fact]
    public void WhenCreatingClosedGenericType_WithName_EqualsResultOfTypeof()
    {
        Type.GetType("System.Collections.Generic.List`1["
                    + "[System.ValueTuple`2["
                        + "[System.String, System.Private.CoreLib],"
                        + "[System.Int32, System.Private.CoreLib]"
                + "], System.Private.CoreLib]"
                + "], System.Private.CoreLib")
            .Is(_expectedClosedGenericType);
    }

    [Fact]
    public void WhenCreatingClosedGenericType_WithoutAssemblyName_EqualsResultOfTypeof()
    {
        Type.GetType("System.Collections.Generic.List`1[[System.ValueTuple`2[[System.String],[System.Int32]]]]")
            .Is(_expectedClosedGenericType);
    }

    [Fact]
    public void WhenCreatingClosedGenericType_WithTypeInstance_EqualsResultOfTypeof()
    {
        var typeOfList = Type.GetType("System.Collections.Generic.List`1");
        var typeOfValueTuple = Type.GetType("System.ValueTuple`2");
        var typeOfValueTupleSpecific
            = typeOfValueTuple?.MakeGenericType(
                Type.GetType("System.String")!,
                Type.GetType("System.Int32")!);

        typeOfList?.MakeGenericType(typeOfValueTupleSpecific!)
            .Is(_expectedClosedGenericType);
    }


    // open generic.
    private readonly Type _expectedOpenGenericType = typeof(ValueTuple<>);

    [Fact]
    public void WhenCreatingOpenGenericType_WithName_EqualsResultOfTypeof()
    {
        Type.GetType("System.ValueTuple`1, System.Private.CoreLib")
            .Is(_expectedOpenGenericType);
    }


    // inner generic.
    private readonly Type _expectedInnerGenericType = typeof(DummyGeneric<,,>);

    [Fact]
    public void WhenCreatingInnerGenericType_WithName_EqualsResultOfTypeof()
    {
        Type.GetType($"{GetType().FullName}+DummyGeneric`3, {GetType().Assembly.FullName}")
            .Is(_expectedInnerGenericType);
    }

    public class DummyGeneric<T1, T2, T3>
    {
    }


    // other assembly generic.
    private readonly Type _expectedOtherAssemblyGenericType = typeof(TheoryData<,,,,,,,,,>);

    [Fact]
    public void WhenCreatingOtherAssemblyGenericType_WithName_EqualsResultOfTypeof()
    {
        Type.GetType("Xunit.TheoryData`10, xunit.core")
            .Is(_expectedOtherAssemblyGenericType);
    }

    [Fact]
    public void WhenCreatingOtherAssemblyGenericType_WithoutAssembly_CannotBe()
    {
        Type.GetType("Examples.Enumerations.Enumeration`1")
            .IsNull();
    }

}

