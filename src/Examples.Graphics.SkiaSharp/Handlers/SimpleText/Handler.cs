using Examples.Graphics.SkiaSharp.Utilities;
using SkiaSharp;

namespace Examples.Graphics.SkiaSharp.Handlers.SimpleText;

public class Handler : IHandler
{
    public Task<int> HandleAsync(IHandler.Parameter parameter, CancellationToken cancellationToken)
    {
        var text = parameter.Text;
        var output = parameter.Output;
        Console.WriteLine($"Rendering \"{text}\" to {output.FullName}...");

        Console.WriteLine($"Platform: {System.Runtime.InteropServices.RuntimeInformation.OSDescription}");
        Console.WriteLine($"Architecture: {System.Runtime.InteropServices.RuntimeInformation.OSArchitecture}");
        Console.WriteLine($"Runtime: {System.Runtime.InteropServices.RuntimeInformation.FrameworkDescription}");
        Console.WriteLine("Platform Color Type: " + SKImageInfo.PlatformColorType);

        // Create the image
        var info = new SKImageInfo(800, 600);
        using var surface = SKSurface.Create(info);
        var canvas = surface.Canvas;

        canvas.Clear(SKColors.White);

        using var paint = new SKPaint
        {
            IsAntialias = true,
            Color = SKColors.Black
        };

        // using var typeface = SKTypeface.FromFamilyName("Arial");
        using var typeface = FontResolver.FromFamilyName("serif");
        using var font = new SKFont(typeface, 100);

        Console.WriteLine($"Font \"{font.Typeface.FamilyName}\", size {font.Size:N0}pt");

        canvas.DrawText("Skia", 20, 140, SKTextAlign.Left, font, paint);

        // Save
        using var image = surface.Snapshot();
        using var data = image.Encode(SKEncodedImageFormat.Jpeg, 100);
        using var stream = output.OpenWrite();
        data.SaveTo(stream);

        Console.WriteLine($"Saved {new FileInfo(output.FullName).Length:N0} bytes to {Path.GetFullPath(output.FullName)}");

        return Task.FromResult(0);
    }
}
