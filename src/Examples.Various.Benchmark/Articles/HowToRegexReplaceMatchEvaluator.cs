using System.ComponentModel;
using System.Text.RegularExpressions;
using BenchmarkDotNet.Attributes;

#pragma warning disable SYSLIB1045 // Use GeneratedRegexAttribute.

namespace Examples.Various.Benchmark.Articles;

[ShortRunJob]
[MemoryDiagnoser]
[MinColumn, MaxColumn]
public partial class HowToRegexReplaceMatchEvaluator
{
    private const string CamelPattern = @"[a-z][A-Z]";
    public readonly string Input = "HeSaidThatThatWasTheTheCorrectAnswer";
    public readonly string Expected = "he_said_that_that_was_the_the_correct_answer";

    [Benchmark]
    public void Replace_StaticMethod()
    {
        var actual = Regex.Replace(Input, CamelPattern, match =>
                $"{match.Groups[0].Value[0]}_{match.Groups[0].Value[1]}").ToLower();

        System.Diagnostics.Debug.Assert(string.Compare(actual, Expected) == 0, $"No match. {actual}");
    }

    [Benchmark]
    public void Replace_StaticMethod_WithSpan()
    {
        var actual = Regex.Replace(Input, @CamelPattern, match =>
            $"{match.Groups[0].Value.AsSpan(0, 1)}_{match.Groups[0].Value.AsSpan(1, 1)}").ToLower();

        System.Diagnostics.Debug.Assert(string.Compare(actual, Expected) == 0, $"No match. {actual}");
    }

#if NET7_0_OR_GREATER

    [Benchmark]
    public void Replace_GeneratedRegexAttribute()
    {
        var actual = MyRegex().Replace(Input, match =>
                $"{match.Groups[0].Value[0]}_{match.Groups[0].Value[1]}").ToLower();

        System.Diagnostics.Debug.Assert(string.Compare(actual, Expected) == 0, $"No match. {actual}");
    }

    [Benchmark]
    public void Replace_GeneratedRegexAttribute_WithAsSpan()
    {
        var actual = MyRegex().Replace(Input, match =>
                $"{match.Groups[0].Value.AsSpan(0, 1)}_{match.Groups[0].Value.AsSpan(1, 1)}").ToLower();

        System.Diagnostics.Debug.Assert(string.Compare(actual, Expected) == 0, $"No match. {actual}");
    }

    [GeneratedRegex(CamelPattern)]
    private static partial Regex MyRegex();

#endif

}
