using System.CommandLine;

namespace Examples.CodeGenerator.Roslyn.Recipes.NotifyPropertyChangedGenerators;

public class RunCommand : Command
{
    public RunCommand() : base("notify", "Runs the NotifyPropertyChangedGenerator sample.")
    {
        SetAction((context) =>
        {
            var target = new TestClass();

            target.PropertyChanged += (sender, args) =>
            {
                Console.WriteLine($"Property changed: {args.PropertyName}, {target.GetType().GetProperty(args.PropertyName!)?.GetValue(target)}");
            };

            target.TestField1 = "New Value";
            target.Field2 = 42;
            target.Field2 = 142;
        });
    }
}
