using Examples.Xunit;

namespace Examples.Metaprogramming.Tests._.System.Reflection.Emit;

public class AssemblyBuilderTests
{
    [Fact]
    public void WhenCallingProgramMain_OutToConsole()
    {
        var mock = new Mock<TextWriter>();
        mock.Setup(x => x.WriteLine((object)"Hello Reflection.Emit World."));

        Type programClass = new ProgramClassBuilder().Build();
        ConsoleHelper.RunWith(mock.Object, ()
            => programClass.GetMethod("Main")!.Invoke(null, null));

        mock.Verify(x => x.WriteLine((object)"Hello Reflection.Emit World."), Times.Exactly(1));
        mock.VerifyNoOtherCalls();
        return;
    }

    [Fact]
    public void WhenUseMyDynamicType_WithDefaultConstructor_ReturnAsExpected()
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
        before.Is(42);
        after.Is(127);
        multiplied.Is(2794);

        return;
    }

    [Fact]
    public void WhenUseMyDynamicType_WithParamConstructor_ReturnAsExpected()
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
        original.Is(5280);
        multiplied.Is(116160);

        return;
    }
}

