using Mono.Cecil;
using Mono.Cecil.Cil;
using Mono.Cecil.Rocks;
using Mono.Cecil.Tests;

namespace Examples.Metaprogramming.Tests._.Mono.Cecil;

/// <summary>
/// Dynamic assembly builder with open generic type.
/// </summary>
public class DemoAssemblyOpenGenericBuilder(string appName = "DynamicAssemblyExample", string typeName = "MyDynamicOpenGeneric`1")
    : BaseModuleDefinitionBuilder
{
    public override ModuleDefinition Build()
    {
        /*
        public class MyDynamicOpenGeneric<T>
        {
            private readonly T? _value;

            public MyDynamicOpenGeneric(T value)
            {
                _value = value;
            }

            public T? Get() => _value;

            public (T?, T?) DoMethod<T1, T2>(T1? param1, T2? param2)
                where T1 : T
                where T2 : T
            {
                return (param1, param2);
            }

        }
        */

        ModuleDefinition module = GetModule(appName);

        TypeDefinition type = new(
            appName,
            typeName,
            TypeAttributes.Public | TypeAttributes.AutoLayout | TypeAttributes.AnsiClass
                | TypeAttributes.BeforeFieldInit,
            module.TypeSystem.Object);
        module.Types.Add(type);

        GenericParameter genericType = new("T", type);
        type.GenericParameters.Add(genericType);

        CustomAttribute x = new(
            module.ImportReference(
                typeof(global::System.Runtime.CompilerServices.NullableContextAttribute).GetConstructor([typeof(byte)])));
        CustomAttribute nullableContextAttribute = new(
            module.ImportReference(
                typeof(global::System.Runtime.CompilerServices.NullableContextAttribute).GetConstructor([typeof(byte)])),
                [01, 00, 02, 00, 00]);
        CustomAttribute nullableAttribute = new(
            module.ImportReference(
                typeof(global::System.Runtime.CompilerServices.NullableAttribute).GetConstructor([typeof(byte)])),
                [01, 00, 00, 00, 00]);
        type.CustomAttributes.Add(nullableContextAttribute);
        type.CustomAttributes.Add(nullableAttribute);

        // private readonly T? _value;
        FieldDefinition valueField = BuildValueField(module, genericType);
        type.Fields.Add(valueField);

        // public MyDynamicOpenGeneric(T value)
        MethodDefinition ctor = BuildConstructor(module, genericType, valueField);
        type.Methods.Add(ctor);

        // public T? Get() => _value;
        MethodDefinition getMethod = BuildGetMethod(module, genericType, valueField);
        type.Methods.Add(getMethod);

        // public (T?, T?) DoMethod<T1, T2>(T1? param1, T2? param2)
        MethodDefinition doMethod = BuildDoMethod(module, genericType);
        type.Methods.Add(doMethod);

        return module;
    }

    private static FieldDefinition BuildValueField(ModuleDefinition module, TypeReference fieldType)
    {
        FieldDefinition field = new(
            "_value",
            FieldAttributes.Private | FieldAttributes.InitOnly,
            fieldType);

        return field;
    }

    private static MethodDefinition BuildConstructor(ModuleDefinition module, GenericParameter genericType, FieldDefinition valueField)
    {
        MethodDefinition ctor = new(
           ".ctor",
           MethodAttributes.HideBySig | MethodAttributes.SpecialName | MethodAttributes.RTSpecialName | MethodAttributes.Public,
           module.TypeSystem.Void);

        ParameterDefinition initValue = new(
            "value",
            ParameterAttributes.None,
            genericType);
        ctor.Parameters.Add(initValue);

        CustomAttribute nullableContextAttribute = new(
                module.ImportReference(
                    typeof(global::System.Runtime.CompilerServices.NullableContextAttribute).GetConstructor([typeof(byte)])),
                    [01, 00, 01, 00, 00]);
        ctor.CustomAttributes.Add(nullableContextAttribute);

        ILProcessor ctorIL = ctor.Body.GetILProcessor();

        ctorIL.Emit(OpCodes.Ldarg_0);
        MethodReference baseCtor = module.ImportReference(
            module.TypeSystem.Object.Resolve().Methods.Single(x => x.Name == ".ctor"));
        ctorIL.Emit(OpCodes.Call, baseCtor!);
        ctorIL.Emit(OpCodes.Nop);
        ctorIL.Emit(OpCodes.Nop);
        ctorIL.Emit(OpCodes.Ldarg_0);
        ctorIL.Emit(OpCodes.Ldarg_1);
        ctorIL.Emit(OpCodes.Stfld, valueField);
        ctorIL.Emit(OpCodes.Ret);

        return ctor;
    }

    private static MethodDefinition BuildGetMethod(ModuleDefinition module, GenericParameter genericType, FieldDefinition valueField)
    {
        MethodDefinition method = new(
             "Get",
            MethodAttributes.HideBySig | MethodAttributes.Public,
            genericType);

        ILProcessor methodIL = method.Body.GetILProcessor();

        methodIL.Emit(OpCodes.Ldarg_0);
        methodIL.Emit(OpCodes.Ldfld, valueField);
        methodIL.Emit(OpCodes.Ret);

        return method;
    }

    private static MethodDefinition BuildDoMethod(ModuleDefinition module, GenericParameter genericType)
    {
        MethodDefinition method = new(
            "DoMethod",
            MethodAttributes.HideBySig | MethodAttributes.Public,
            module.TypeSystem.Void)
        {
            CallingConvention = MethodCallingConvention.Generic
        };

        CustomAttribute nullableContextAttribute = new(
            module.ImportReference(
                typeof(global::System.Runtime.CompilerServices.NullableContextAttribute).GetConstructor([typeof(byte)])),
                [01, 00, 00, 00, 00]);
        CustomAttribute nullableAttributeConstraint = new(
            module.ImportReference(
                typeof(global::System.Runtime.CompilerServices.NullableAttribute).GetConstructor([typeof(byte)])),
                [01, 00, 01, 00, 00]);
        CustomAttribute nullableAttributeArray = new(
            module.ImportReference(
                typeof(global::System.Runtime.CompilerServices.NullableAttribute).GetConstructor([typeof(byte[])])),
                [01, 00, 03, 00, 00, 00, 00, 02, 02, 00, 00]);
        CustomAttribute nullableAttributeParam = new(
            module.ImportReference(
                typeof(global::System.Runtime.CompilerServices.NullableAttribute).GetConstructor([typeof(byte)])),
                [01, 00, 02, 00, 00]);

        method.CustomAttributes.Add(nullableContextAttribute);

        GenericParameter genericTypeT1 = new GenericParameter("T1", method)
            .Add(new GenericParameterConstraint(genericType)
                .Add(nullableAttributeConstraint)
                );
        GenericParameter genericTypeT2 = new GenericParameter("T2", method)
            .Add(new GenericParameterConstraint(genericType)
                .Add(nullableAttributeConstraint)
                );
        method.GenericParameters.Add(genericTypeT1);
        method.GenericParameters.Add(genericTypeT2);

        GenericInstanceType returnType = module
            .ImportReference(typeof(ValueTuple<,>))
            .MakeGenericInstanceType([genericType, genericType]);
        method.ReturnType = returnType;
        method.MethodReturnType.Add(nullableAttributeArray);

        ParameterDefinition param1 = new ParameterDefinition(
            "param1",
            ParameterAttributes.None,
            genericTypeT1)
            .Add(nullableAttributeParam)
            ;
        ParameterDefinition param2 = new ParameterDefinition(
            "param2",
            ParameterAttributes.None,
            genericTypeT2)
            .Add(nullableAttributeParam)
            ;
        method.Parameters.Add(param1);
        method.Parameters.Add(param2);

        // [0] valuetype [System.Private.CoreLib]System.ValueTuple`2<!T, !T>
        method.Body.Variables.Add(new(returnType));
        method.Body.InitLocals = true;

        ILProcessor methodIL = method.Body.GetILProcessor();

        methodIL.Emit(OpCodes.Nop);
        methodIL.Emit(OpCodes.Ldarg_1);
        methodIL.Emit(OpCodes.Box, genericTypeT1);
        methodIL.Emit(OpCodes.Unbox_Any, genericType);

        methodIL.Emit(OpCodes.Ldarg_2);
        methodIL.Emit(OpCodes.Box, genericTypeT2);
        methodIL.Emit(OpCodes.Unbox_Any, genericType);

        // 	IL_0017: newobj instance void valuetype [System.Runtime]System.ValueTuple`2<!T, !T>::.ctor(!0, !1) /* 0A000051 */
        MethodReference newTuple = module.ImportReference(
                module.ImportReference(typeof(ValueTuple<,>))
                    .Resolve().Methods.First(x => x.Name == ".ctor"))
                .MakeGeneric([genericType, genericType]);
        methodIL.Emit(OpCodes.Newobj, newTuple);

        methodIL.Emit(OpCodes.Stloc_0);
        var il001f = methodIL.Create(OpCodes.Ldloc_0);
        methodIL.Emit(OpCodes.Br_S, il001f);
        methodIL.Append(il001f);
        methodIL.Emit(OpCodes.Ret);

        return method;
    }

}
