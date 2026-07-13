namespace Examples.Graphics.SkiaSharp.Handlers.QuickStart;

public interface IHandler
{
    record struct Parameter(
        FileInfo Output
    );

    Task<int> HandleAsync(Parameter parameter, CancellationToken cancellationToken);
}
