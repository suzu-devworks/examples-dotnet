using System.CommandLine;

RootCommand rootCommand = new("Sample implementation of Examples.Analyzer.Roslyn.")
{
    new Examples.Analyzer.Roslyn.Tutorials.RunCommand(),
};

return await rootCommand.Parse(args).InvokeAsync();
