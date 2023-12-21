using Mono.Cecil;

namespace Examples.Metaprogramming.Tests._.Mono.Cecil;

/// <summary>
/// Dynamic assembly builder base class.
/// </summary>
public abstract class BaseModuleDefinitionBuilder
{
    public abstract ModuleDefinition Build();

    public BaseModuleDefinitionBuilder UseModule(ModuleDefinition module)
    {
        _module = module;
        return this;
    }
    private ModuleDefinition? _module;

    protected ModuleDefinition GetModule(string appName) => _module ?? CreateModuleDefinition(appName);

    private static ModuleDefinition CreateModuleDefinition(string appName)
    {
        AssemblyDefinition assembly = AssemblyDefinition.CreateAssembly(
            new AssemblyNameDefinition(appName, new Version()),
            appName,
            ModuleKind.Dll);

        return assembly.MainModule;
    }

}
