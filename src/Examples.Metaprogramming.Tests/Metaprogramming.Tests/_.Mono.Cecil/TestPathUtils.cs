using System.Reflection;

namespace Examples.Metaprogramming.Tests._.Mono.Cecil;

public static class TestPathUtils
{
    public static string GetOutPath() =>
        Path.Combine(
            Path.GetDirectoryName(Assembly.GetEntryAssembly()!.Location)!,
            "out");

    public static string GetOutPath(string filePath) =>
        Path.Combine(
            GetOutPath(),
            Path.GetFileName(filePath)
        );
}
