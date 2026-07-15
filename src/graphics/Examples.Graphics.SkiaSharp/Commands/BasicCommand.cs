using System.CommandLine;
using Examples.Graphics.SkiaSharp.Handlers.Basic;

namespace Examples.Graphics.SkiaSharp.Commands;

/// <summary>
/// A basic example of using SkiaSharp to draw lines on an image.
/// </summary>
/// <seealso href="https://github.com/mono/SkiaSharp/blob/main/samples/Basic/DockerConsole/SkiaSharpSample/Program.cs"/>
public class BasicCommand : Command
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

    public BasicCommand(IHandler handler) : base("basic", "A basic example of using SkiaSharp to draw lines on an image.")
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
