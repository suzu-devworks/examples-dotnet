using Mono.Cecil;

namespace Examples.Metaprogramming.Tests._.Mono.Cecil;

internal static class Extensions
{

    public static string GetOutPath(this global::System.Reflection.Assembly assembly, string fileName) =>
        Path.Combine(
            Path.GetDirectoryName(assembly.Location)!,
            "out",
            fileName);

    public static void WriteFixture(this ModuleDefinition module, string path)
    {
        Directory.CreateDirectory(Path.GetDirectoryName(path)!);
        File.Delete(path);
        module.Write(path);

        return;
    }

    public static GenericParameter Clone(this GenericParameter parameter)
        => new(parameter.Name, parameter.Owner);

    public static GenericParameter Add(this GenericParameter parameter, GenericParameterConstraint where)
    {
        parameter.Constraints.Add(where);
        return parameter;
    }

    public static T Add<T>(this T constraint, CustomAttribute item)
        where T : ICustomAttributeProvider
    {
        constraint.CustomAttributes.Add(item);
        return constraint;
    }

}
