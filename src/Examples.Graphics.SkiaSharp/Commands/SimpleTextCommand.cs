using System.CommandLine;
using Examples.Graphics.SkiaSharp.Handlers.SimpleText;

namespace Examples.Graphics.SkiaSharp.Commands;

public class SimpleTextCommand : Command
{
    public Option<FileInfo> Output { get; } = new("--output", "-o")
    {
        Description = "The output file path for the generated image.",
        DefaultValueFactory = _ => new FileInfo("output.jpg")
    };

    public Argument<string> Text { get; } = new("text")
    {
        Description = "The text to draw on the image.",
    };

    public SimpleTextCommand(IHandler handler) : base("text", "An example of using SkiaSharp to draw text on an image.")
    {
        Options.Add(Output);
        Arguments.Add(Text);

        SetAction(async (parsed, cancellationToken) =>
        {
            IHandler.Parameter parameter = new(
                Output: parsed.GetValue(Output)!,
                Text: parsed.GetValue(Text)!
            );

            return await handler.HandleAsync(parameter, cancellationToken);
        });
    }
}
