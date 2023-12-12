using System.Reflection;
using Mono.Cecil;

namespace Examples.Metaprogramming.Tests._.Mono.Cecil;

internal static class Extensions
{
    public static void WriteFixture(this ModuleDefinition module, string path)
    {
        Directory.CreateDirectory(Path.GetDirectoryName(path)!);
        File.Delete(path);
        module.Write(path);

        return;
    }

    public static string GetOutPath(this Assembly assembly, string fileName) =>
        Path.Combine(
            Path.GetDirectoryName(assembly.Location)!,
            "out",
            fileName);

}
