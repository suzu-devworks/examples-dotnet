namespace Examples.Metaprogramming.ReflectionEmit.Tests.Learns;

public class DemoAssemblyBuilderTests
{

    [Fact]
    public void WhenUsingGeneratedTypes_WithDefaultConstructor_WorksAsExpected()
    {
        Type myDynamicTypeClass = new DemoAssemblyBuilder().Build();

        // Create an instance of MyDynamicType using the default
        // constructor.
        dynamic instance = Activator.CreateInstance(myDynamicTypeClass)!;

        // Display the value of the property, then change it to 127 and
        // display it again. Use null to indicate that the property
        // has no index.
        int before = instance.Number;
        instance.Number = 127;
        int after = instance.Number;

        // Call MyMethod, passing 22, and display the return value, 22
        // times 127. Arguments must be passed as an array, even when
        // there is only one.
        int multiplied = instance.MyMethod(22);

        // assert.
        Assert.Equal(42, before);
        Assert.Equal(127, after);
        Assert.Equal(2794, multiplied);
    }

    [Fact]
    public void WhenUsingGeneratedTypes_WithParameterConstructor_WorksAsExpected()
    {
        Type myDynamicTypeClass = new DemoAssemblyBuilder().Build();

        // Create an instance of MyDynamicType using the constructor
        // that specifies m_Number. The constructor is identified by
        // matching the types in the argument array. In this case,
        // the argument array is created on the fly. Display the
        // property value.
        dynamic instance = Activator.CreateInstance(myDynamicTypeClass, new object[] { 5280 })!;

        // Display the value of the property, then change it to 127 and
        // display it again. Use null to indicate that the property
        // has no index.
        int original = instance.Number;

        // Call MyMethod, passing 22, and display the return value, 22
        // times 127. Arguments must be passed as an array, even when
        // there is only one.
        int multiplied = instance.MyMethod(22);

        // assert.
        Assert.Equal(5280, original);
        Assert.Equal(5280 * 22, multiplied);
    }
}
