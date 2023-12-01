using System.Text;
using BenchmarkDotNet.Attributes;

namespace Examples.Benchmark.Articles;

[MemoryDiagnoser]
public class HowToConcatenateStrings
{
    private IEnumerable<string> _data = default!;

    [Params(10000)]
    public int N;

    [GlobalSetup]
    public void Setup()
    {
        _data = Enumerable.Range(0, N).Select(x => x.ToString());
    }

    [Benchmark]
    public string WhenUsingOperator() => _data.Aggregate((a, b) => a + "," + b);

    [Benchmark]
    public string WhenUsingStringConcat() => _data.Aggregate((a, b) => string.Concat(a, ",", b));

    [Benchmark]
    public string WhenUsingStringJoin() => string.Join(",", _data);

    [Benchmark]
    public string WhenUsingStringBuilderAppend() => _data
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
    public async Task<string> WhenUsingStringWriterWriteAsync()
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
