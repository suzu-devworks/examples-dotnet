using Mono.Cecil;
using Mono.Cecil.Cil;

namespace Examples.Metaprogramming.Tests._.Mono.Cecil;

/// <summary>
/// Dynamic assembly builder described in Microsoft docs.
/// </summary>
public class DemoAssemblyBuilder(string appName = "DynamicAssemblyExample", string typeName = "MyDynamicType")
{
    public ModuleDefinition Build()
    {
        /*
        public class MyDynamicType
        {
            private int m_number;

            public MyDynamicType() : this(42) {}
            public MyDynamicType(int initNumber)
            {
                m_number = initNumber;
            }

            public int Number
            {
                get { return m_number; }
                set { m_number = value; }
            }

            public int MyMethod(int multiplier)
            {
                return m_number * multiplier;
            }
        }
        */

        ModuleDefinition module = _module ?? CreateModuleDefinition(appName);

        var type = new TypeDefinition(
            appName,
            typeName,
            TypeAttributes.Public,
            module.TypeSystem.Object);
        module.Types.Add(type);

        // private int m_number;
        FieldDefinition numberField = BuilderNumberField(module);
        type.Fields.Add(numberField);

        // public MyDynamicType(int initNumber)
        MethodDefinition ctor1 = BuildConstructor1(module, numberField);
        type.Methods.Add(ctor1);

        // public MyDynamicType() : this(42) {}
        MethodDefinition ctor0 = BuildConstructor0(module, ctor1);
        type.Methods.Add(ctor0);

        // public int Number { get { ... } set { ... } }
        PropertyDefinition numberProperty = BuildPropertyNumber(module, numberField);
        type.Properties.Add(numberProperty);
        type.Methods.Add(numberProperty.GetMethod);
        type.Methods.Add(numberProperty.SetMethod);

        // public int MyMethod(int multiplier)
        MethodDefinition myMethod = BuildMethodMyMethod(module, numberField);
        type.Methods.Add(myMethod);

        // Finish the type.

        return module;
    }

    private static ModuleDefinition CreateModuleDefinition(string appName)
    {
        AssemblyDefinition assembly = AssemblyDefinition.CreateAssembly(
            new AssemblyNameDefinition(appName, new Version()),
            appName,
            ModuleKind.Dll);

        return assembly.MainModule;
    }

    private static FieldDefinition BuilderNumberField(ModuleDefinition module)
    {
        // Add a private field of type int (Int32).
        FieldDefinition field = new(
            "m_number",
            FieldAttributes.Private,
            module.TypeSystem.Int32);

        return field;
    }

    private static MethodDefinition BuildConstructor1(ModuleDefinition module, FieldReference numberField)
    {
        MethodDefinition ctor1 = new(
            ".ctor",
            MethodAttributes.HideBySig | MethodAttributes.SpecialName | MethodAttributes.RTSpecialName | MethodAttributes.Public,
            module.TypeSystem.Void);

        ParameterDefinition initNumber = new(
            "initNumber",
            ParameterAttributes.None,
            module.TypeSystem.Int32);
        ctor1.Parameters.Add(initNumber);

        ILProcessor ctor1IL = ctor1.Body.GetILProcessor();
        // For a constructor, argument zero is a reference to the new
        // instance. Push it on the stack before calling the base
        // class constructor. Specify the default constructor of the
        // base class (System.Object) by passing an empty array of
        // types (Type.EmptyTypes) to GetConstructor.
        ctor1IL.Emit(OpCodes.Ldarg_0);
        MethodReference baseCtor = module.ImportReference(
            module.TypeSystem.Object.Resolve().Methods.Single(x => x.Name == ".ctor"));
        ctor1IL.Emit(OpCodes.Call, baseCtor!);
        // Push the instance on the stack before pushing the argument
        // that is to be assigned to the private field m_number.
        ctor1IL.Emit(OpCodes.Ldarg_0);
        ctor1IL.Emit(OpCodes.Ldarg_1);
        ctor1IL.Emit(OpCodes.Stfld, numberField);
        ctor1IL.Emit(OpCodes.Ret);

        return ctor1;
    }

    private static MethodDefinition BuildConstructor0(ModuleDefinition module, MethodReference ctor1)
    {
        // Define a default constructor that supplies a default value
        // for the private field. For parameter types, pass the empty
        // array of types or pass null.
        MethodDefinition ctor0 = new(
             ".ctor",
            MethodAttributes.HideBySig | MethodAttributes.SpecialName | MethodAttributes.RTSpecialName | MethodAttributes.Public,
            module.TypeSystem.Void);

        ILProcessor ctor0IL = ctor0.Body.GetILProcessor();
        // For a constructor, argument zero is a reference to the new
        // instance. Push it on the stack before pushing the default
        // value on the stack, then call constructor ctor1.
        ctor0IL.Emit(OpCodes.Ldarg_0);
        ctor0IL.Emit(OpCodes.Ldc_I4_S, (sbyte)42);
        ctor0IL.Emit(OpCodes.Call, ctor1);
        ctor0IL.Emit(OpCodes.Ret);

        return ctor0;
    }

    private static PropertyDefinition BuildPropertyNumber(ModuleDefinition module, FieldReference numberField)
    {
        // Define a property named Number that gets and sets the private
        // field.
        //
        // The last argument of DefineProperty is null, because the
        // property has no parameters. (If you don't specify null, you must
        // specify an array of Type objects. For a parameterless property,
        // use the built-in array with no elements: Type.EmptyTypes)
        PropertyDefinition property = new(
            "Number",
            PropertyAttributes.HasDefault,
            module.TypeSystem.Int32);

        // The property "set" and property "get" methods require a special
        // set of attributes.
        MethodAttributes attributes = MethodAttributes.Public |
            MethodAttributes.SpecialName | MethodAttributes.HideBySig;

        // Define the "get" accessor method for Number. The method returns
        // an integer and has no arguments. (Note that null could be
        // used instead of Types.EmptyTypes)
        MethodDefinition getter = new(
            "get_Number",
            attributes,
            module.TypeSystem.Int32);

        ILProcessor getterIL = getter.Body.GetILProcessor();
        // For an instance property, argument zero is the instance. Load the
        // instance, then load the private field and return, leaving the
        // field value on the stack.
        getterIL.Emit(OpCodes.Ldarg_0);
        getterIL.Emit(OpCodes.Ldfld, numberField);
        getterIL.Emit(OpCodes.Ret);

        // Define the "set" accessor method for Number, which has no return
        // type and takes one argument of type int (Int32).
        MethodDefinition setter = new(
            "set_Number",
            attributes,
            module.TypeSystem.Void);

        ParameterDefinition value = new(
            "value",
            ParameterAttributes.None,
            module.TypeSystem.Int32);
        setter.Parameters.Add(value);

        ILProcessor setterIL = setter.Body.GetILProcessor();
        // Load the instance and then the numeric argument, then store the
        // argument in the field.
        setterIL.Emit(OpCodes.Ldarg_0);
        setterIL.Emit(OpCodes.Ldarg_1);
        setterIL.Emit(OpCodes.Stfld, numberField);
        setterIL.Emit(OpCodes.Ret);

        // Last, map the "get" and "set" accessor methods to the
        // PropertyBuilder. The property is now complete.
        property.GetMethod = getter;
        property.SetMethod = setter;

        return property;
    }

    private static MethodDefinition BuildMethodMyMethod(ModuleDefinition module, FieldReference numberField)
    {
        // Define a method that accepts an integer argument and returns
        // the product of that integer and the private field m_number. This
        // time, the array of parameter types is created on the fly.
        MethodDefinition method = new(
            "MyMethod",
            MethodAttributes.Public,
            module.TypeSystem.Int32);

        ParameterDefinition multiplier = new(
            "multiplier",
            ParameterAttributes.None,
            module.TypeSystem.Int32);
        method.Parameters.Add(multiplier);

        ILProcessor methodIL = method.Body.GetILProcessor();
        // To retrieve the private instance field, load the instance it
        // belongs to (argument zero). After loading the field, load the
        // argument one and then multiply. Return from the method with
        // the return value (the product of the two numbers) on the
        // execution stack.
        methodIL.Emit(OpCodes.Ldarg_0);
        methodIL.Emit(OpCodes.Ldfld, numberField);
        methodIL.Emit(OpCodes.Ldarg_1);
        methodIL.Emit(OpCodes.Mul);
        methodIL.Emit(OpCodes.Ret);

        return method;
    }


    public DemoAssemblyBuilder Module(ModuleDefinition module)
    {
        _module = module;
        return this;
    }

    private ModuleDefinition? _module;

}
