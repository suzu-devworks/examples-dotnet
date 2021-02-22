using System;
using System.IO;
using System.Reflection;
using System.Runtime.CompilerServices;
using ChainingAssertion;
using Xunit;

#pragma warning disable IDE0051

namespace Examples.Unloadability
{
    public class AssemblyLoadContextTests
    {
        private readonly string AssemblyPath =
            Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "Examples.dll");

        [Fact]
        void TestUnloadAssembly()
        {

            var result = ExecuteAndUnload(AssemblyPath, out WeakReference testAlcWeakRef);
            var i = 0;
            for (; testAlcWeakRef.IsAlive && (i < 10); ++i)
            {
                GC.Collect();
                GC.WaitForPendingFinalizers();
            }

            result.Is(111);
            i.Is(x => x < 10);
            testAlcWeakRef.IsAlive.IsFalse();

            return;
        }

        [Fact]
        void TestNotUnloadAssembly()
        {
            var result = ExecuteAndUnload(AssemblyPath, out WeakReference testAlcWeakRef, out Type typeRef);
            var i = 0;
            for (; testAlcWeakRef.IsAlive && (i < 10); ++i)
            {
                GC.Collect();
                GC.WaitForPendingFinalizers();
            }

            result.Is(111);
            i.Is(10);
            testAlcWeakRef.IsAlive.IsTrue();
            typeRef.IsNotNull();

            return;
        }


        [MethodImpl(MethodImplOptions.NoInlining)]
        static int ExecuteAndUnload(string assemblyPath, out WeakReference alcWeakRef)
        {
            return ExecuteAndUnload(assemblyPath, out alcWeakRef, out Type _);
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        static int ExecuteAndUnload(string assemblyPath, out WeakReference alcWeakRef, out Type typeRef)
        {

            var alc = new CollectibleAssemblyLoadContext();
            alcWeakRef = new WeakReference(alc, trackResurrection: true);

            var asm = alc.LoadFromAssemblyPath(assemblyPath);
            var type = asm.GetType("Examples.Unloadability.Dummy");
            var method = type.GetMethod("Add");

            typeRef = type;

            var instance = Activator.CreateInstance(type);
            var args = new object[] { 10, 101 };
            var result = (int)method.Invoke(instance, args);

            alc.Unload();

            return result;
        }
    }
}
