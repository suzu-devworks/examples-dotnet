#!/usr/bin/env dotnet run
#:package ConsoleAppFramework@5.*

// Chap05-02.cs
// [GlobalOptions](https://github.com/Cysharp/ConsoleAppFramework#globaloptions)
//
// By calling ConfigureGlobalOptions on ConsoleAppBuilder,
// you can define global options that are enabled for all commands.

using ConsoleAppFramework;

var app = ConsoleApp.Create();

// builder: Func<ref ConsoleApp.GlobalOptionsBuilder, object>
app.ConfigureGlobalOptions((ref builder) =>
{
    var dryRun = builder.AddGlobalOption<bool>("--dry-run");
    var verbose = builder.AddGlobalOption<bool>("-v|--verbose");
    var intParameter = builder.AddRequiredGlobalOption<int>("--int-parameter", "integer parameter");
    // return value stored to ConsoleAppContext.GlobalOptions
    return new GlobalOptions(dryRun, verbose, intParameter);
});

app.Add("", (int x, int y, ConsoleAppContext context) =>
{
    var globalOptions = (GlobalOptions)context.GlobalOptions!;
    Console.WriteLine(globalOptions + ":" + (x, y));
});

app.Run(args);

internal record GlobalOptions(bool DryRun, bool Verbose, int IntParameter);
