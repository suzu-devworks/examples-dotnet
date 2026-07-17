namespace Examples.Graphics.SkiaSharp.Handlers.SimpleText;

public interface IHandler
{
    record struct Parameter(
        FileInfo Output,
        string Text
    );

    Task<int> HandleAsync(Parameter parameter, CancellationToken cancellationToken);
}
