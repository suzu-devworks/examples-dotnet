using Examples.Metaprogramming.Helpers;
using Examples.Metaprogramming.Runtime.Loader;
using Mono.Cecil;

namespace Examples.Metaprogramming.MonoCecil.Tests.Learns;

[Collection(TestCollectionNames.UseSystemConsole)]
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
        ConsoleRedirection console = new();

        using (var context = new DisposableAssemblyLoadContext(nameof(AssemblyDefinitionTests)))
        {
            var assembly = context.LoadFromAssemblyPath(path);
            var type = assembly.GetType("MonoCecilExample.Program");

            assembly.EntryPoint!.Invoke(null, null);
        }

        Assert.Contains("Hello Mono.Cecil World", console.GetOutput());
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
        Assert.Equal(42, before);
        Assert.Equal(127, after);
        Assert.Equal(2794, multiplied);
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
        Assert.Equal("42<=", actual);
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

            actualValue = closedGenericType.GetMethod("Get")?.Invoke(instance, []);

            var openGenericMethod = closedGenericType!.GetMethod("DoMethod");
            var closedGenericMethod = openGenericMethod!.MakeGenericMethod(typeof(int), typeof(int));
            actualTuple = closedGenericMethod.Invoke(instance, [(object)456, (object)789]);

        }

        // assert.
        Assert.Equal(123, actualValue);
        Assert.Equal((456, 789), actualTuple);
    }

}
