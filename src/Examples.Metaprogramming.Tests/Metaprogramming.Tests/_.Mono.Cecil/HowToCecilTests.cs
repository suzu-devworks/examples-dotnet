using System.Runtime.InteropServices;
using Examples.Metaprogramming.Runtime.Loader;
using Examples.Xunit;
using Mono.Cecil;
using Mono.Cecil.Cil;
using Xunit.Abstractions;
using Xunit.Sdk;

namespace Examples.Metaprogramming.Tests._.Mono.Cecil;

/// <summary>
/// Tests the usage method described in the official Wiki
/// </summary>
/// <seealso href="https://github.com/jbevain/cecil/wiki/HOWTO" />
public partial class HowToCecilTests(ITestOutputHelper output)
{
    // ```
    // dotnet test --logger "console;verbosity=detailed"
    // ```

    [Fact]
    public void WhenOpeningModuleAndReferencingItsTopLevelType()
    {
        var runtimeDir = RuntimeEnvironment.GetRuntimeDirectory();
        var path = Path.Combine(runtimeDir, "System.Threading.dll");
        output.WriteLine($"Types in \"{path}\" is :");

        var mock = new Mock<TextWriter>();
        mock.Setup(x => x.WriteLine((string?)"System.Threading.BarrierPostPhaseException"));
        mock.Setup(x => x.WriteLine((string?)"System.Threading.Barrier"));
        mock.Setup(x => x.WriteLine((string?)"System.Threading.CountdownEvent"));
        mock.Setup(x => x.WriteLine((string?)"System.Threading.HostExecutionContext"));
        mock.Setup(x => x.WriteLine((string?)"System.Threading.HostExecutionContextManager"));
        mock.Setup(x => x.WriteLine((string?)"System.Threading.LockCookie"));
        mock.Setup(x => x.WriteLine((string?)"System.Threading.ReaderWriterLock"));

        // Act.
        ConsoleHelper.RunWith(mock.Object, () => PrintTypes(path));

        mock.Verify(x => x.WriteLine((string?)"System.Threading.BarrierPostPhaseException"), Times.Exactly(1));
        mock.Verify(x => x.WriteLine((string?)"System.Threading.Barrier"), Times.Exactly(1));
        mock.Verify(x => x.WriteLine((string?)"System.Threading.CountdownEvent"), Times.Exactly(1));
        mock.Verify(x => x.WriteLine((string?)"System.Threading.HostExecutionContext"), Times.Exactly(1));
        mock.Verify(x => x.WriteLine((string?)"System.Threading.HostExecutionContextManager"), Times.Exactly(1));
        mock.Verify(x => x.WriteLine((string?)"System.Threading.LockCookie"), Times.Exactly(1));
        mock.Verify(x => x.WriteLine((string?)"System.Threading.ReaderWriterLock"), Times.Exactly(1));
        // mock.VerifyNoOtherCalls();
        return;

        void PrintTypes(string fileName)
        {
            using ModuleDefinition module = ModuleDefinition.ReadModule(fileName);
            foreach (TypeDefinition type in module.Types)
            {
                if (!type.IsPublic)
                    continue;

                Console.WriteLine(type.FullName);
                output.WriteLine($"\t{type.FullName}");
            }
        }
    }

    [Fact]
    public void WheCheckIfATypeHasACertainCustomAttribute()
    {
        var path = typeof(HowToCecilTests).Assembly.Location;
        output.WriteLine($"Read assembly is: \"{path}\"");

        using AssemblyDefinition assembly = AssemblyDefinition.ReadAssembly(path);

        TypeDefinition typeDef = assembly.MainModule.GetType(typeof(HowToCecilTests).FullName)
            .NestedTypes.FirstOrDefault(t => t.FullName == typeof(Fixture).FullName!.Replace("+", "/"))
                ?? throw new XunitException("Type not found.");

        // Act
        var actual = TryGetCustomAttribute(
            typeDef,
            typeof(Foo.IgnoreAttribute).FullName!,
            out var ignoreAttribute);

        actual.IsTrue();
        ignoreAttribute.IsNotNull();
        ignoreAttribute!.AttributeType.FullName.Is(typeof(Foo.IgnoreAttribute).FullName);
        ignoreAttribute!.ConstructorArguments.Count.Is(1);
        ignoreAttribute!.ConstructorArguments[0].Value.Is("Not working yet");

        return;

        static bool TryGetCustomAttribute(TypeDefinition type,
            string attributeType, out CustomAttribute? result)
        {
            result = null;
            if (!type.HasCustomAttributes)
                return false;

            foreach (CustomAttribute attribute in type.CustomAttributes)
            {
                if (attribute.AttributeType.FullName != attributeType)
                    continue;

                result = attribute;
                return true;
            }

            return false;
        }
    }

    [Fact]
    public void WhenInsertAnILInstructionBeforeAnother()
    {
        var path = typeof(HowToCecilTests).Assembly.Location;
        output.WriteLine($"Read assembly is: \"{path}\"");

        using AssemblyDefinition assembly = AssemblyDefinition.ReadAssembly(path);

        TypeDefinition typeDef = assembly.MainModule.GetType(typeof(HowToCecilTests).FullName)
            .NestedTypes.FirstOrDefault(t => t.FullName == typeof(Fixture).FullName!.Replace("+", "/"))
                ?? throw new XunitException("Type not found.");

        MethodDefinition method = typeDef
            .Methods.FirstOrDefault(x => x.Name == "Sum")
                ?? throw new XunitException("Method not found.");

        var original = new Fixture(10).Sum(10);

        // Modify module.
#if DEBUG
        method.Body.Instructions.Count.Is(9);
#else
        method.Body.Instructions.Count.Is(5);
#endif

        Modify(method);

#if DEBUG
        method.Body.Instructions.Count.Is(11);
#else
        method.Body.Instructions.Count.Is(7);
#endif

        // write copy module.
        var outPath = typeof(HowToCecilTests).Assembly.GetOutPath(
             $"{Path.GetFileNameWithoutExtension(typeof(HowToCecilTests).Assembly.Location)}.$HowTo.dll");
        assembly.MainModule.WriteFixture(outPath);

        // Run new assembly(module).
        int newValue = 0;
        using (var context = new DisposableAssemblyLoadContext(nameof(HowToCecilTests)))
        {
            var newAssembly = context.LoadFromAssemblyPath(outPath);
            var type = newAssembly.GetType(typeof(Fixture).FullName!);
            dynamic newInstance = Activator.CreateInstance(type!, 10)!;
            newValue = newInstance.Sum(10);
        }

        original.Is(20);
        newValue.Is(20 * 2);

        return;

        static void Modify(MethodDefinition method)
        {
            // === return x + y; with Debug ===
            // IL_0000: nop
            // IL_0001: ldarg.0
            // IL_0002: ldfld int32 Examples.Metaprogramming.Tests._.Mono.Cecil.HowToCecilTests/Fixture::'<x>P' /* 04000017 */
            // IL_0007: ldarg.1
            // IL_0008: add         // <== target

            // <<< return (x + y) * 2;
            // IL_0009: ldc.i4.2
            // IL_000a: mul
            // ===
            // >>>

            // IL_000b: stloc.0     // <== target.Next
            // IL_000c: br.s IL_000e
            // IL_000e: ldloc.0
            // IL_000f: ret

            // === return x + y; with Release ===
            // IL_0000: ldarg.0
            // IL_0001: ldfld System.Int32 Examples.Metaprogramming.Tests._.Mono.Cecil.HowToCecilTests / Fixture::< x > P
            // IL_0006: ldarg.1
            // IL_0007: add         // <== target

            // <<< return (x + y) * 2;
            // IL_0009: ldc.i4.2
            // IL_000a: mul
            // ===
            // >>>

            // IL_0008: ret         // <== target.Next

            var processor = method.Body.GetILProcessor();
            var target = method.Body.Instructions.FirstOrDefault(x => x.OpCode == OpCodes.Add)?.Next
                ?? throw new XunitException("opcode not found.");

            processor.InsertBefore(target, Instruction.Create(OpCodes.Ldc_I4_2));
            processor.InsertBefore(target, Instruction.Create(OpCodes.Mul));

            return;
        }
    }


}
