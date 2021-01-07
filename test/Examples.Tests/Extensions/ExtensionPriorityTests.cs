using System;
using System.Collections.Generic;
using System.Linq;
using Examples.A;
using Examples.Extensions.A;
using Xunit;

#pragma warning disable IDE0051
#pragma warning disable IDE0052

namespace Examples.Extensions.A
{
    static class Extension
    {
        public static string DoExtentOfA<T>(this IEnumerable<T> _)
            => throw new InvalidOperationException();

        public static string DoExtentOfB<T>(this IEnumerable<T> _)
            => throw new InvalidOperationException();

        public static string DoExtentOfC<T>(this IEnumerable<T> _)
            => $"Extend-C: {typeof(Extension).FullName}";

        public static string DoExtentOfD<T>(this IEnumerable<T> _)
            => $"Extend-D: {typeof(Extension).FullName}";
    }
}

namespace Examples.A
{
    static class Extension
    {
        public static string DoExtentOfA<T>(this IEnumerable<T> _)
            => throw new InvalidOperationException();

        public static string DoExtentOfB<T>(this IEnumerable<T> _)
            => throw new InvalidOperationException();

        //CS0121 vs Examples.Extensions.A.Extension
        // public static string DoExtentOfC<T>(this IEnumerable<T> _)
        //     => $"Extend-C: {typeof(Extension).FullName}";
    }
}

namespace Examples
{
    static class Extension
    {
        public static string DoExtentOfA<T>(this IEnumerable<T> _)
            => throw new InvalidOperationException();

        public static string DoExtentOfB<T>(this IEnumerable<T> _)
            => $"Extend-B: {typeof(Extension).FullName}";
    }
}

namespace Examples.Extensions
{
    static class Extension
    {
        public static string DoExtentOfA<T>(this IEnumerable<T> _)
            => $"Extend-A: {typeof(Extension).FullName}";
    }

    //CS0121 vs Examples.Extensions.Extension
    // static class Extension2
    // {
    //     public static string DoExtentOfA<T>(this IEnumerable<T> _)
    //        => $"Extend-A: {typeof(Extension).FullName}";
    // }
}

namespace Examples.Extensions
{
    public class ExtensionPriorityTests
    {
        [Fact]
        void TestFoundExtensions()
        {
            var data = Enumerable.Range(1, 100);

            var resultOfA = data.DoExtentOfA();
            Console.WriteLine(resultOfA);

            var resultOfB = data.DoExtentOfB();
            Console.WriteLine(resultOfB);

            var resultOfC = data.DoExtentOfC();
            Console.WriteLine(resultOfC);

            var resultOfD = data.DoExtentOfD();
            Console.WriteLine(resultOfD);

            return;
        }

    }
}
