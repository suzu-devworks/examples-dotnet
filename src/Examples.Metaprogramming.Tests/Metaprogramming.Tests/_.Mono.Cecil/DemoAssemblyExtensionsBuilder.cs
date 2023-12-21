using Mono.Cecil;
using Mono.Cecil.Cil;

namespace Examples.Metaprogramming.Tests._.Mono.Cecil;

/// <summary>
/// Dynamic assembly builder with extension methods.
/// </summary>
public sealed class DemoAssemblyExtensionsBuilder(string appName = "DynamicAssemblyExample", string typeName = "MyDynamicExtensions")
    : BaseModuleDefinitionBuilder
{
    public override ModuleDefinition Build()
    {
        /*
        public static class MyDynamicExtensions
        {
            public static string DoExtension(this MyDynamicType source, string appended)
            {
                var number = source.Number.ToString();
                var value = string.Concat(number, appended);
                return value;
            }
        }
        */

        ModuleDefinition module = GetModule(appName);

        CustomAttribute nullableContextAttribute = new(
            module.ImportReference(
                typeof(global::System.Runtime.CompilerServices.NullableContextAttribute).GetConstructor([typeof(byte)])),
            [01, 00, 01, 00, 00]);
        CustomAttribute extensionAttribute = new(
            module.ImportReference(
                typeof(global::System.Runtime.CompilerServices.ExtensionAttribute).GetConstructor(Type.EmptyTypes)));

        var type = new TypeDefinition(
            appName,
            typeName,
            TypeAttributes.Public | TypeAttributes.AutoLayout | TypeAttributes.AnsiClass
                | TypeAttributes.Abstract | TypeAttributes.Sealed | TypeAttributes.BeforeFieldInit,
            module.TypeSystem.Object);
        module.Types.Add(type);

        type.CustomAttributes.Add(extensionAttribute);

        MethodDefinition extension = new(
            "DoExtension",
            MethodAttributes.HideBySig | MethodAttributes.Static | MethodAttributes.Public,
            module.TypeSystem.String);
        type.Methods.Add(extension);

        extension.CustomAttributes.Add(nullableContextAttribute);
        extension.CustomAttributes.Add(extensionAttribute);

        TypeDefinition myDynamicType = module.GetType("DynamicAssemblyExample.MyDynamicType");

        ParameterDefinition source = new(
            "source",
            ParameterAttributes.None,
            module.ImportReference(myDynamicType));

        ParameterDefinition appended = new(
            "appended",
            ParameterAttributes.None,
            module.TypeSystem.String);

        extension.Parameters.Add(source);
        extension.Parameters.Add(appended);

        extension.Body.Variables.Add(new(module.TypeSystem.String));
        extension.Body.Variables.Add(new(module.TypeSystem.String));
        extension.Body.Variables.Add(new(module.TypeSystem.Int32));
        extension.Body.Variables.Add(new(module.TypeSystem.String));
        extension.Body.InitLocals = true;

        var getNumber = myDynamicType.Methods.Where(x => x.Name == "get_Number").First();
        var intToString = typeof(int).GetMethod(nameof(string.ToString), Type.EmptyTypes);
        var stringConcat = typeof(string).GetMethod(nameof(string.Concat), [typeof(string), typeof(string)]);

        var il = extension.Body.GetILProcessor();

        il.Emit(OpCodes.Nop);
        il.Emit(OpCodes.Ldarg_0);
        il.Emit(OpCodes.Callvirt, module.ImportReference(getNumber));
        il.Emit(OpCodes.Stloc_2);
        il.Emit(OpCodes.Ldloca_S, (byte)2);
        il.Emit(OpCodes.Call, module.ImportReference(intToString));
        il.Emit(OpCodes.Stloc_0);
        il.Emit(OpCodes.Ldloc_0);
        il.Emit(OpCodes.Ldarg_1);
        il.Emit(OpCodes.Call, module.ImportReference(stringConcat));
        il.Emit(OpCodes.Stloc_1);
        il.Emit(OpCodes.Ldloc_1);
        il.Emit(OpCodes.Stloc_3);

        var il001c = il.Create(OpCodes.Ldloc_3);
        il.Emit(OpCodes.Br_S, il001c);
        il.Append(il001c);
        il.Emit(OpCodes.Ret);

        return module;
    }

}
