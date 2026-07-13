using System.CommandLine;

RootCommand rootCommand = new("SkiaSharp Examples")
{
    new Examples.Graphics.SkiaSharp.Commands.BasicCommand(
        new Examples.Graphics.SkiaSharp.Handlers.Basic.Handler()),
    new Examples.Graphics.SkiaSharp.Commands.SimpleTextCommand(
        new Examples.Graphics.SkiaSharp.Handlers.SimpleText.Handler()),
    new Examples.Graphics.SkiaSharp.Commands.QuickStartCommand(
        new Examples.Graphics.SkiaSharp.Handlers.QuickStart.Handler()),
};

return await rootCommand.Parse(args).InvokeAsync();
