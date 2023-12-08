using System.Reflection;
using System.Reflection.Emit;

namespace Examples.Metaprogramming.Tests._.System.Reflection.Emit;

/// <summary>
/// Dynamic assembly builder described in Microsoft docs.
/// </summary>
/// <see href="https://learn.microsoft.com/en-us/dotnet/api/system.reflection.emit.assemblybuilder?view=net-8.0"/>
public static class DemoAssemblyBuilder
{
    public static Type Build()
    {
        // This code creates an assembly that contains one type,
        // named "MyDynamicType", that has a private field, a property
        // that gets and sets the private field, constructors that
        // initialize the private field, and a method that multiplies
        // a user-supplied number by the private field value and returns
        // the result. In C# the type might look like this:
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

        var appName = new AssemblyName("DynamicAssemblyExample");
        AssemblyBuilder assembly =
            AssemblyBuilder.DefineDynamicAssembly(
                appName,
                AssemblyBuilderAccess.Run);

        // The module name is usually the same as the assembly name.
        ModuleBuilder module = assembly.DefineDynamicModule(
            appName.Name ?? "DynamicAssemblyExample");

        TypeBuilder typeBuilder = module.DefineType(
            "MyDynamicType",
             TypeAttributes.Public);

        // private int m_number;
        FieldBuilder numberField = BuilderNumberField(typeBuilder);

        // public MyDynamicType(int initNumber)
        ConstructorBuilder ctor1 = BuildConstructor1(typeBuilder, numberField);

        // public MyDynamicType() : this(42) {}
        BuildConstructor0(typeBuilder, ctor1);

        // public int Number { get { ... } set { ... } }
        BuildPropertyNumber(typeBuilder, numberField);

        // public int MyMethod(int multiplier)
        BuildMethodMyMethod(typeBuilder, numberField);

        // Finish the type.
        Type type = typeBuilder.CreateType();

        return type;
    }

    private static FieldBuilder BuilderNumberField(TypeBuilder typeBuilder)
    {
        // Add a private field of type int (Int32).
        return typeBuilder.DefineField(
            "m_number",
            typeof(int),
            FieldAttributes.Private);
    }

    private static ConstructorBuilder BuildConstructor1(TypeBuilder typeBuilder, FieldBuilder numberField)
    {
        // Define a constructor that takes an integer argument and
        // stores it in the private field.
        Type[] parameterTypes = [typeof(int)];
        ConstructorBuilder ctor1 = typeBuilder.DefineConstructor(
            MethodAttributes.Public,
            CallingConventions.Standard,
            parameterTypes);

        ILGenerator ctor1IL = ctor1.GetILGenerator();
        // For a constructor, argument zero is a reference to the new
        // instance. Push it on the stack before calling the base
        // class constructor. Specify the default constructor of the
        // base class (System.Object) by passing an empty array of
        // types (Type.EmptyTypes) to GetConstructor.
        ctor1IL.Emit(OpCodes.Ldarg_0);
        ConstructorInfo? ci = typeof(object).GetConstructor(Type.EmptyTypes);
        ctor1IL.Emit(OpCodes.Call, ci!);
        // Push the instance on the stack before pushing the argument
        // that is to be assigned to the private field m_number.
        ctor1IL.Emit(OpCodes.Ldarg_0);
        ctor1IL.Emit(OpCodes.Ldarg_1);
        ctor1IL.Emit(OpCodes.Stfld, numberField);
        ctor1IL.Emit(OpCodes.Ret);

        return ctor1;
    }

    private static ConstructorBuilder BuildConstructor0(TypeBuilder typeBuilder, ConstructorBuilder ctor1)
    {
        // Define a default constructor that supplies a default value
        // for the private field. For parameter types, pass the empty
        // array of types or pass null.
        ConstructorBuilder ctor0 = typeBuilder.DefineConstructor(
            MethodAttributes.Public,
            CallingConventions.Standard,
            Type.EmptyTypes);

        ILGenerator ctor0IL = ctor0.GetILGenerator();
        // For a constructor, argument zero is a reference to the new
        // instance. Push it on the stack before pushing the default
        // value on the stack, then call constructor ctor1.
        ctor0IL.Emit(OpCodes.Ldarg_0);
        ctor0IL.Emit(OpCodes.Ldc_I4_S, 42);
        ctor0IL.Emit(OpCodes.Call, ctor1);
        ctor0IL.Emit(OpCodes.Ret);

        return ctor0;
    }


    private static (PropertyBuilder property, MethodBuilder getter, MethodBuilder setter)
        BuildPropertyNumber(TypeBuilder typeBuilder, FieldBuilder numberField)
    {
        // Define a property named Number that gets and sets the private
        // field.
        //
        // The last argument of DefineProperty is null, because the
        // property has no parameters. (If you don't specify null, you must
        // specify an array of Type objects. For a parameterless property,
        // use the built-in array with no elements: Type.EmptyTypes)
        PropertyBuilder property = typeBuilder.DefineProperty(
            "Number",
            PropertyAttributes.HasDefault,
            typeof(int),
            null);

        // The property "set" and property "get" methods require a special
        // set of attributes.
        MethodAttributes attributes = MethodAttributes.Public |
            MethodAttributes.SpecialName | MethodAttributes.HideBySig;

        // Define the "get" accessor method for Number. The method returns
        // an integer and has no arguments. (Note that null could be
        // used instead of Types.EmptyTypes)
        MethodBuilder getter = typeBuilder.DefineMethod(
            "get_Number",
            attributes,
            typeof(int),
            Type.EmptyTypes);

        ILGenerator getterIL = getter.GetILGenerator();
        // For an instance property, argument zero is the instance. Load the
        // instance, then load the private field and return, leaving the
        // field value on the stack.
        getterIL.Emit(OpCodes.Ldarg_0);
        getterIL.Emit(OpCodes.Ldfld, numberField);
        getterIL.Emit(OpCodes.Ret);

        // Define the "set" accessor method for Number, which has no return
        // type and takes one argument of type int (Int32).
        MethodBuilder setter = typeBuilder.DefineMethod(
            "set_Number",
            attributes,
            null,
            new Type[] { typeof(int) });

        ILGenerator setterIL = setter.GetILGenerator();
        // Load the instance and then the numeric argument, then store the
        // argument in the field.
        setterIL.Emit(OpCodes.Ldarg_0);
        setterIL.Emit(OpCodes.Ldarg_1);
        setterIL.Emit(OpCodes.Stfld, numberField);
        setterIL.Emit(OpCodes.Ret);

        // Last, map the "get" and "set" accessor methods to the
        // PropertyBuilder. The property is now complete.
        property.SetGetMethod(getter);
        property.SetSetMethod(setter);

        return (property, getter, setter);
    }

    private static MethodBuilder BuildMethodMyMethod(TypeBuilder typeBuilder, FieldBuilder numberField)
    {

        // Define a method that accepts an integer argument and returns
        // the product of that integer and the private field m_number. This
        // time, the array of parameter types is created on the fly.
        MethodBuilder method = typeBuilder.DefineMethod(
            "MyMethod",
            MethodAttributes.Public,
            typeof(int),
            new Type[] { typeof(int) });

        ILGenerator methodIL = method.GetILGenerator();
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


}
