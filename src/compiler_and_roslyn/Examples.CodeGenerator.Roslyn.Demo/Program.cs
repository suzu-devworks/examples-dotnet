
using System.CommandLine;

RootCommand rootCommand = new("Sample implementation of Examples.CodeGenerator.Roslyn.")
{
    new Examples.CodeGenerator.Roslyn.Introducing.Hello.RunCommand(),
    new Examples.CodeGenerator.Roslyn.Recipes.EnumDescriptionGenerators.RunCommand(),
    new Examples.CodeGenerator.Roslyn.Recipes.NotifyPropertyChangedGenerators.RunCommand(),
};

return await rootCommand.Parse(args).InvokeAsync();
