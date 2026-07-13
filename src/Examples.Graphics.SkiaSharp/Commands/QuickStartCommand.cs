using System.CommandLine;
using Examples.Graphics.SkiaSharp.Handlers.QuickStart;

namespace Examples.Graphics.SkiaSharp.Commands;

/// <summary>
/// A basic example of using SkiaSharp to draw lines on an image.
/// </summary>
/// <seealso href="https://swharden.com/csdv/skiasharp/quickstart-console/#1-create-a-console-app"/>
public class QuickStartCommand : Command
{
    public Option<FileInfo> Output { get; } = new("--output", "-o")
    {
        Description = "The output file path for the generated image.",
        DefaultValueFactory = _ => new FileInfo("output.png")
    };

    public QuickStartCommand(IHandler handler) : base("quickstart", "A basic example of using SkiaSharp to draw lines on an image.")
    {
        Options.Add(Output);

        SetAction(async (parsed, cancellationToken) =>
        {
            IHandler.Parameter parameter = new(
                Output: parsed.GetValue(Output)!
            );

            return await handler.HandleAsync(parameter, cancellationToken);
        });
    }
}
