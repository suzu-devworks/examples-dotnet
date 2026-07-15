using System.Runtime.CompilerServices;

namespace Examples.CodeGenerator.T4;

public static class Project
{
    public static string GetRootPath() => GetRootPathInternal();

    private static string GetRootPathInternal([CallerFilePath] string sourceFilePath = "")
    {
        return Directory.GetParent(sourceFilePath)?.FullName ?? "";
    }
}
