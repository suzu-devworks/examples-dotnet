using System.CommandLine;

namespace Examples.Analyzer.Roslyn.Tutorials;

#pragma warning disable CS0219 // variable is assigned but its value is never used

public class RunCommand : Command
{
    public RunCommand() : base("make-const", "Runs the MakeConstAnalyzer sample.")
    {
        SetAction((context) =>
        {
            // This program is designed to intentionally trigger a warning from the analyzer.

            const
            // This program is designed to intentionally trigger a warning from the analyzer.

            int i = 1;
            const int j = 2;
            int k = i + j;

            // uncomment for analyzer test:
            //int x = "abc";

            object s = "abc";

            const string s2 = "abc";

            const string? item = "xyz";

            Console.WriteLine($"k = {k}, s = {s}, s2 = {s2}, item = {item}");
        });
    }
}
