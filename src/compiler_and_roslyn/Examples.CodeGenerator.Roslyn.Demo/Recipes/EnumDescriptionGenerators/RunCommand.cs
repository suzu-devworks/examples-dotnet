using System.CommandLine;

namespace Examples.CodeGenerator.Roslyn.Recipes.EnumDescriptionGenerators;

public class RunCommand : Command
{
    public RunCommand() : base("enum", "Runs the EnumDescriptionGenerator sample.")
    {
        SetAction((context) =>
        {
            foreach (var value in Enum.GetValues<TestEnum>())
            {
                Console.WriteLine($"Value: {value}, Description: {value.GetDescription()}");
            }
        });
    }
}
