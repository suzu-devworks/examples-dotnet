namespace Examples.Metaprogramming.Helpers;

public class ConsoleRedirection : IDisposable
{
    private readonly TextWriter _originalOut;
    private readonly StringWriter _stringWriter;

    public ConsoleRedirection()
    {
        _originalOut = Console.Out;
        _stringWriter = new StringWriter();
        Console.SetOut(_stringWriter);
    }

    public string GetOutput() => _stringWriter.ToString();

    public void Dispose()
    {
        Console.SetOut(_originalOut);
        _stringWriter.Dispose();
        GC.SuppressFinalize(this);
    }
}
