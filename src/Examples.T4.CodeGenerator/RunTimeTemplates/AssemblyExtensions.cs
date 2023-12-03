using System.Reflection;
using System.Text.RegularExpressions;

namespace Examples.T4.CodeGenerator.RunTimeTemplates;

public static class AssemblyExtensions
{
    public static void RunTemplateGeneratorSelector(this Assembly assembly, string[] args)
    {
        var types = assembly.GetTypes()
            .Where(t => t.GetInterfaces().Any(i => i == typeof(ITemplateGenerator)))
            .OrderBy(t => t.FullName)
            ;

        while (true)
        {
            Console.WriteLine("Select Generator:", args);
            foreach (var (type, i) in types.Select((x, i) => (x, i)))
            {
                Console.WriteLine($"[{i,2}] {type.FullName}");
            }
            Console.WriteLine();

            Console.Write("input number (Q to quit):");
            var selected = Console.ReadLine();
            Console.WriteLine($"{selected} selected.");
            Console.WriteLine();

            if (Regex.IsMatch(selected ?? "", @"^[Qq]$"))
            {
                break;
            }

            if (int.TryParse(selected ?? "", out var num))
            {
                var type = types.ElementAtOrDefault(num);
                if (type is null)
                {
                    continue;
                }
                var generator = Activator.CreateInstance(type) as ITemplateGenerator;
                generator?.Generate(new TemplateCodeWriter());
            }
        }

        return;
    }
}
