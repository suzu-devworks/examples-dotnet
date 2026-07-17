#!/usr/bin/env dotnet run
#:package ConsoleAppFramework@5.*

// Chap05-04.cs
// [Custom Value Converter](https://github.com/Cysharp/ConsoleAppFramework#custom-value-converter)
//
// To perform custom binding to existing types that do not support ISpanParsable<T>,
// you can create and set up a custom parser.

using System.Numerics;
using ConsoleAppFramework;

ConsoleApp.Run(args, ([Vector3Parser] Vector3 position) => Console.WriteLine(position));

[AttributeUsage(AttributeTargets.Parameter)]
internal class Vector3ParserAttribute : Attribute, IArgumentParser<Vector3>
{
    public static bool TryParse(ReadOnlySpan<char> s, out Vector3 result)
    {
        Span<Range> ranges = stackalloc Range[3];
        var splitCount = s.Split(ranges, ',');
        if (splitCount != 3)
        {
            result = default;
            return false;
        }

        float x;
        float y;
        float z;
        if (float.TryParse(s[ranges[0]], out x) && float.TryParse(s[ranges[1]], out y) && float.TryParse(s[ranges[2]], out z))
        {
            result = new Vector3(x, y, z);
            return true;
        }

        result = default;
        return false;
    }
}
