using System.Text.RegularExpressions;
using BenchmarkDotNet.Attributes;

#pragma warning disable SYSLIB1045 // Use GeneratedRegexAttribute.

namespace Examples.Various.Benchmark.Articles;

[ShortRunJob]
[MemoryDiagnoser]
[MinColumn, MaxColumn]
public partial class HowToRegexIsMatch
{
    private const string AddressPattern = @"^([a-zA-Z0-9_/\.\-\?\+])+\@([a-zA-Z0-9]+[a-zA-Z0-9\-]*\.)+[a-zA-Z0-9\-]+$";

    public static IEnumerable<string> InputValues() => [
                "user@example.com",
                "test.name@subdomain.example.co.jp",
                "invalid@",
                "no-at-sign.com",
                "special!char@domain.com",
            ];

    [Benchmark]
    [ArgumentsSource(nameof(InputValues))]
    public bool IsMatch_NewInstance(string input) =>
            new Regex(AddressPattern).IsMatch(input);

    [Benchmark]
    [ArgumentsSource(nameof(InputValues))]
    public bool IsMatch_StaticInstance(string input) =>
            StaticRegex.IsMatch(input);

    private static readonly Regex StaticRegex = new(AddressPattern);


    [Benchmark]
    [ArgumentsSource(nameof(InputValues))]
    public bool IsMatch_StaticInstanceCompiled(string input) =>
            CompiledStaticRegex.IsMatch(input);

    private static readonly Regex CompiledStaticRegex = new(AddressPattern, RegexOptions.Compiled);


    [Benchmark]
    [ArgumentsSource(nameof(InputValues))]
    public bool IsMatch_StaticMethod(string input) =>
            Regex.IsMatch(input, AddressPattern);

    [Benchmark]
    [ArgumentsSource(nameof(InputValues))]
    public bool IsMatch_StaticMethodCompiled(string input) =>
        Regex.IsMatch(input, AddressPattern, RegexOptions.Compiled);

#if NET7_0_OR_GREATER

    [Benchmark]
    [ArgumentsSource(nameof(InputValues))]
    public bool IsMatch_GeneratedRegexAttribute(string input) =>
            MyRegex().IsMatch(input);

    [GeneratedRegex(AddressPattern)]
    private static partial Regex MyRegex();

    [Benchmark]
    [ArgumentsSource(nameof(InputValues))]
    public bool IsMatch_GeneratedRegexAttributeCompiled(string input) =>
            MyRegexCompiled().IsMatch(input);

    [GeneratedRegex(AddressPattern, RegexOptions.Compiled)]
    private static partial Regex MyRegexCompiled();

#endif

}
