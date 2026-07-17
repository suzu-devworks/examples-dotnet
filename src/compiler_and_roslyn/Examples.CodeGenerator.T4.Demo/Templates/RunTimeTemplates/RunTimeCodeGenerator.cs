using System.Text.RegularExpressions;

namespace Examples.CodeGenerator.T4.Templates.RunTimeTemplates;

public partial class RunTimeCodeGenerator
{
    public static async Task RunLoopAsync<T>() where T : class
    {
        var generators = EnumerateTemplateGenerator<T>().ToList();
        if (generators.Count == 0)
        {
            WriteLine("No generators found.");
            return;
        }

        var writer = new ConsoleCodeWriter();

        while (true)
        {
            WriteLine("========================================");
            WriteImportantLine("Select Generator:");
            foreach (var (type, i) in generators.Select((x, i) => (x, i)))
            {
                WriteLine($"[{i,2}] {type.FullName}");
            }
            WriteLine("========================================");
            var selected = Input("input number (Q to quit):");

            if (QuitExpression().IsMatch(selected ?? ""))
            {
                break;
            }

            if (!int.TryParse(selected ?? "", out var num))
            {
                continue;
            }

            if (num < 0 || num >= generators.Count)
            {
                WriteError("Invalid selection. Please try again.");
                continue;
            }

            var generatorType = generators[num];

            WriteImportantLine($"[{num,2}] {generatorType.FullName} selected.");
            WriteLine();

            var generator = Activator.CreateInstance(generatorType) as ITemplateGenerator
                ?? throw new InvalidOperationException($"Failed to create instance of {generatorType.FullName}.");
            await generator.GenerateAsync(writer);
        }
    }


    private static IOrderedEnumerable<Type> EnumerateTemplateGenerator<T>()
        where T : class
        => typeof(T).Assembly.GetTypes()
                .Where(t => t.GetInterfaces().Any(i => i == typeof(ITemplateGenerator)))
                .OrderBy(t => t.FullName);

    private static void WriteLine(string? text = null)
        => System.Console.WriteLine(text);

    private static void WriteImportantLine(string? text = null)
    {
        System.Console.ForegroundColor = ConsoleColor.Yellow;
        System.Console.WriteLine(text);
        System.Console.ResetColor();
    }

    private static void WriteError(string? text = null)
    {
        System.Console.ForegroundColor = ConsoleColor.DarkRed;
        System.Console.WriteLine(text);
        System.Console.ResetColor();
    }

    public static string Input(string? text = null)
    {
        System.Console.Write(text);
        return System.Console.ReadLine() ?? "";
    }

    [GeneratedRegex(@"^[Qq]$")]
    private static partial Regex QuitExpression();
}
