using Examples.Metaprogramming.Runtime.Loader;
using Examples.Xunit;
using Mono.Cecil;
using Xunit.Abstractions;

namespace Examples.Metaprogramming.Tests._.Mono.Cecil;

public partial class AssemblyDefinitionTests(ITestOutputHelper output)
{
    [Fact]
    public void WhenCallingGeneratedHelloWorldProgram_OutToConsoleAsExpected()
    {
        var path = typeof(AssemblyDefinitionTests).Assembly.GetOutPath(
            $"{Path.GetFileNameWithoutExtension(typeof(AssemblyDefinitionTests).Assembly.Location)}.$HelloWorld.dll");

        // Write new console module.
        ModuleDefinition module = new HelloWorldBuilder().Build();
        module.WriteFixture(path);
        output.WriteLine($"Wrote assembly is: \"{path}\"");

        // Run new assembly(module).
        var mock = new Mock<TextWriter>();
        mock.Setup(x => x.WriteLine((object)"Hello Mono.Cecil World."));

        using (var context = new DisposableAssemblyLoadContext(nameof(AssemblyDefinitionTests)))
        {
            var assembly = context.LoadFromAssemblyPath(path);
            var type = assembly.GetType("MonoCecilExample.Program");

            ConsoleHelper.RunWith(mock.Object, ()
                => assembly.EntryPoint!.Invoke(null, null));
        }

        mock.Verify(x => x.WriteLine((object)"Hello Mono.Cecil World."), Times.Exactly(1));
        mock.VerifyNoOtherCalls();

        return;
    }

    [Fact]
    public void WhenUsingGeneratedTypes_WithDefaultConstructor_WorksAsExpected()
    {
        var path = typeof(AssemblyDefinitionTests).Assembly.GetOutPath(
            $"{Path.GetFileNameWithoutExtension(typeof(AssemblyDefinitionTests).Assembly.Location)}.$MyDynamicType.dll");

        // Write new module.
        ModuleDefinition module = new DemoAssemblyBuilder().Build();
        module.WriteFixture(path);
        output.WriteLine($"Wrote assembly is: \"{path}\"");

        // Run new assembly(module).
        int? before = null;
        int? after = null;
        int? multiplied = null;
        using (var context = new DisposableAssemblyLoadContext(nameof(AssemblyDefinitionTests)))
        {
            var assembly = context.LoadFromAssemblyPath(path);
            var type = assembly.GetType("DynamicAssemblyExample.MyDynamicType");

            // Create an instance of MyDynamicType using the default
            // constructor.
            dynamic instance = Activator.CreateInstance(type!)!;

            // Display the value of the property, then change it to 127 and
            // display it again. Use null to indicate that the property
            // has no index.
            before = instance.Number;
            instance.Number = 127;
            after = instance.Number;

            // Call MyMethod, passing 22, and display the return value, 22
            // times 127. Arguments must be passed as an array, even when
            // there is only one.
            multiplied = instance.MyMethod(22);
        }

        // assert.
        before.Is(42);
        after.Is(127);
        multiplied.Is(127 * 22);

        return;
    }

    [Fact]
    public void WhenCallingGeneratedExtensionMethod_WorksAsExpected()
    {
        var path = typeof(AssemblyDefinitionTests).Assembly.GetOutPath(
            $"{Path.GetFileNameWithoutExtension(typeof(AssemblyDefinitionTests).Assembly.Location)}.$MyExtensions.dll");

        // Write new module.
        ModuleDefinition baseModule = new DemoAssemblyBuilder().Build();
        ModuleDefinition module = new DemoAssemblyExtensionsBuilder().UseModule(baseModule).Build();
        module.WriteFixture(path);
        output.WriteLine($"Wrote assembly is: \"{path}\"");

        // Run new assembly(module).
        object? actual = null;
        using (var context = new DisposableAssemblyLoadContext(nameof(AssemblyDefinitionTests)))
        {
            var assembly = context.LoadFromAssemblyPath(path);
            var type = assembly.GetType("DynamicAssemblyExample.MyDynamicType");
            var extension = assembly.GetType("DynamicAssemblyExample.MyDynamicExtensions");

            var instance = Activator.CreateInstance(type!)!;
            actual = extension!.GetMethod("DoExtension")!.Invoke(null, (object[])[instance, "<="]);
        }

        // assert.
        actual!.IsInstanceOf<string>()
            .Is("42<=");

        return;
    }

    [Fact]
    public void WhenUsingGeneratedOpenGenericType_WorksAsExpected()
    {
        var path = typeof(AssemblyDefinitionTests).Assembly.GetOutPath(
            $"{Path.GetFileNameWithoutExtension(typeof(AssemblyDefinitionTests).Assembly.Location)}.$MyGeneric.dll");

        // Write new module.
        ModuleDefinition module = new DemoAssemblyOpenGenericBuilder().Build();
        module.WriteFixture(path);
        output.WriteLine($"Wrote assembly is: \"{path}\"");

        // Run new assembly(module).
        object? actualValue = null;
        object? actualTuple = null;
        using (var context = new DisposableAssemblyLoadContext(nameof(AssemblyDefinitionTests)))
        {
            var assembly = context.LoadFromAssemblyPath(path);
            var type = assembly.GetType("DynamicAssemblyExample.MyDynamicOpenGeneric`1");

            var closedGenericType = type!.MakeGenericType(typeof(int));
            var instance = Activator.CreateInstance(closedGenericType!, 123)!;

            actualValue = instance.InvokeAs(closedGenericType, "Get");

            var openGenericMethod = closedGenericType!.GetMethod("DoMethod");
            var closedGenericMethod = openGenericMethod!.MakeGenericMethod(typeof(int), typeof(int));
            actualTuple = closedGenericMethod.Invoke(instance, [(object)456, (object)789]);

        }

        // assert.
        actualValue!.IsInstanceOf<int>()
            .Is(123);
        actualTuple!.IsInstanceOf<ValueTuple<int, int>>()
            .Is(value => value.Item1 == 456 && value.Item2 == 789);

        return;
    }

}
