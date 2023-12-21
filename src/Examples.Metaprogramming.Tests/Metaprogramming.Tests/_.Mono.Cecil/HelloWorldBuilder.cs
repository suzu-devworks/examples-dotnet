using Mono.Cecil;
using Mono.Cecil.Cil;

namespace Examples.Metaprogramming.Tests._.Mono.Cecil;

/// <summary>
/// Simple hello world assembly builder.
/// </summary>
public sealed class HelloWorldBuilder(string appName = "MonoCecilExample")
{
    public ModuleDefinition Build()
    {
        // This code creates an assembly containing a program with one main function.
        /*
        public class Program
        {
            public static void Main()
            {
                Console.WriteLine((object)"Hello Mono.Cecil World.")
            }
        }
        */

        using var assembly = AssemblyDefinition.CreateAssembly(
            new AssemblyNameDefinition(appName, new Version()),
            appName,
            ModuleKind.Console);

        var module = assembly.MainModule;

        var type = new TypeDefinition(
            appName,
            "Program",
            TypeAttributes.Public,
            module.ImportReference(typeof(object)));
        module.Types.Add(type);

        var method = new MethodDefinition(
            "Main",
            MethodAttributes.Static,
            module.ImportReference(typeof(void)));
        type.Methods.Add(method);

        var writeLine = typeof(Console).GetMethod(nameof(Console.WriteLine), [typeof(object)]);

        var il = method.Body.GetILProcessor();
        il.Emit(OpCodes.Ldstr, "Hello Mono.Cecil World.");
        il.Emit(OpCodes.Call, module.ImportReference(writeLine));
        il.Emit(OpCodes.Ret);

        module.EntryPoint = method;

        return module;
    }

}

