using System.Text;
using BenchmarkDotNet.Attributes;

namespace Examples.Various.Benchmark.Articles;

[MemoryDiagnoser]
public class HowToConcatenateStrings
{
    private const int N = 10000;
    private readonly IEnumerable<string> _data = default!;

    public HowToConcatenateStrings()
    {
        _data = Enumerable.Range(0, N).Select(x => x.ToString());
    }

    [Benchmark]
    public string Using_PlusOperator() => _data.Aggregate((a, b) => a + "," + b);

    [Benchmark]
    public string Using_String_Concat() => _data.Aggregate((a, b) => string.Concat(a, ",", b));

    [Benchmark]
    public string Using_String_Join() => string.Join(",", _data);

    [Benchmark]
    public string Using_StringBuilder_Append() => _data
        .Aggregate(
            new StringBuilder(),
            (builder, data) =>
            {
                if (builder.Length > 0) { builder.Append(','); }
                builder.Append(data);
                return builder;
            },
            builder => builder.ToString()
        );

    [Benchmark]
    public async Task<string> Using_StringWriter_WriteAsync()
    {
        var builder = new StringBuilder();
        using (var writer = new StringWriter(builder))
        {
            foreach (var chunk in _data)
            {
                await writer.WriteAsync(chunk);
            }
        }

        return builder.ToString();
    }
}
