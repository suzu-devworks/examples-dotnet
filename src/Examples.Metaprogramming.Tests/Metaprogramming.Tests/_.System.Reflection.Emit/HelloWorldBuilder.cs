using System.Reflection;
using System.Reflection.Emit;

namespace Examples.Metaprogramming.Tests._.System.Reflection.Emit;

/// <summary>
/// Simple hello world assembly builder.
/// </summary>
public sealed class HelloWorldBuilder(string appName = "DynamicAssemblyExample")
{
    public Type Build()
    {
        // This code creates an assembly containing a program with one main function.
        /*
        public class Program
        {
            public static void Main()
            {
                Console.WriteLine((object)"Hello Reflection.Emit World.")
            }
        }
        */

        AssemblyName asmName = new(appName);

        AssemblyBuilder assembly = AssemblyBuilder.DefineDynamicAssembly(
            asmName,
            AssemblyBuilderAccess.Run); //RunAndSave Not Found.

        ModuleBuilder module = assembly.DefineDynamicModule(
            asmName.Name!);

        TypeBuilder type = module.DefineType(
            "Program",
            TypeAttributes.Public);

        MethodBuilder method = type.DefineMethod(
            "Main",
            MethodAttributes.Public | MethodAttributes.Static);

        ILGenerator mainIL = method.GetILGenerator();

        MethodInfo writeLine = typeof(Console).GetMethod(
            name: nameof(Console.WriteLine),
            [typeof(object)])!;

        mainIL.Emit(OpCodes.Ldstr, "Hello Reflection.Emit World.");
        mainIL.Emit(OpCodes.Call, writeLine);
        mainIL.Emit(OpCodes.Ret);

        Type program = type.CreateType();
        // assembly.SetEntryPoint(method);  //Not Found.
        // assembly.Save($"{APP_NAME}.exe"); //Not Found.

        return program;
    }

}
