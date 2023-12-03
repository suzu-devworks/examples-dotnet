using System.Text.RegularExpressions;
using BenchmarkDotNet.Attributes;

namespace Examples.Benchmark.Articles;

#pragma warning disable SYSLIB1045

[MemoryDiagnoser]
[MinColumn, MaxColumn]
public partial class HowToRegexReplaceMatchEvaluator
{
    private const string CAMEL_PATTERN = @"[a-z][A-Z]";

    public static readonly string Input = "HeSaidThatThatWasTheTheCorrectAnswer";
    public static readonly string Expected = "he_said_that_that_was_the_the_correct_answer";


    [Benchmark]
    public void WhenNormal()
    {
        var s = Regex.Replace(Input, CAMEL_PATTERN, match =>
            $"{match.Groups[0].Value[0]}_{match.Groups[0].Value[1]}").ToLower();

        if (s != Expected)
        {
            throw new Exception($"No match. {s}");
        }
    }

    [Benchmark]
    public void WhenWithAsSpan()
    {
        var s = Regex.Replace(Input, @CAMEL_PATTERN, match =>
            $"{match.Groups[0].Value.AsSpan(0, 1).ToString()}_{match.Groups[0].Value.AsSpan(1, 1).ToString()}").ToLower();

        if (s != Expected)
        {
            throw new Exception($"No match. {s}");
        }
    }

#if NET7_0_OR_GREATER

    [Benchmark]
    public void WhenUsingStaticInstance()
    {
        var s = CamelMatchRegex.Replace(Input, match =>
            $"{match.Groups[0].Value[0]}_{match.Groups[0].Value[1]}").ToLower();

        if (s != Expected)
        {
            throw new Exception($"No match. {s}");
        }
    }

    [Benchmark]
    public void WhenUsingStaticInstance_WithAsSpan()
    {
        var s = CamelMatchRegex.Replace(Input, match =>
            $"{match.Groups[0].Value.AsSpan(0, 1).ToString()}_{match.Groups[0].Value.AsSpan(1, 1).ToString()}").ToLower();

        if (s != Expected)
        {
            throw new Exception($"No match. {s}");
        }
    }

    private static readonly Regex CamelMatchRegex = MyRegex();

    [GeneratedRegex(CAMEL_PATTERN, RegexOptions.Compiled)]
    private static partial Regex MyRegex();

#endif

}
