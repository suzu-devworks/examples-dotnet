using System.Text.RegularExpressions;
using BenchmarkDotNet.Attributes;

namespace Examples.Benchmark.Articles;

#pragma warning disable SYSLIB1045

[MemoryDiagnoser]
[MinColumn, MaxColumn]
public partial class HowToRegExIsMatch
{
    private const string ADDRESS_PATTERN = @"^([a-zA-Z0-9_/\.\-\?\+])+\@([a-zA-Z0-9]+[a-zA-Z0-9\-]*\.)+[a-zA-Z0-9\-]+$";

    public static IEnumerable<string> ValuesForInput() => new[] {
            "AAAAAAAAAAA@contoso.com",
            "AAAAAAAAAAaaaaaaaaaa!@contoso.com",
            "AAAAAAAAAAaaaaaaaaaacontoso.com",
        };

    [Benchmark]
    [ArgumentsSource(nameof(ValuesForInput))]
    public bool WhenUsingNewInstance(string input) =>
        new Regex(ADDRESS_PATTERN).IsMatch(input);


    [Benchmark]
    [ArgumentsSource(nameof(ValuesForInput))]
    public bool WhenUsingStaticInstance(string input) =>
        StaticRegex.IsMatch(input);

    private static readonly Regex StaticRegex = new(ADDRESS_PATTERN);


    [Benchmark]
    [ArgumentsSource(nameof(ValuesForInput))]
    public bool WhenUsingCompiledStaticInstance(string input) =>
        CompiledStaticRegex.IsMatch(input);

    private static readonly Regex CompiledStaticRegex = new(ADDRESS_PATTERN, RegexOptions.Compiled);


    [Benchmark]
    [ArgumentsSource(nameof(ValuesForInput))]
    public bool WhenUsingStaticMethod(string input) =>
        Regex.IsMatch(input, ADDRESS_PATTERN);

    [Benchmark]
    [ArgumentsSource(nameof(ValuesForInput))]
    public bool WhenUsingCompiledStaticMethod(string input) =>
        Regex.IsMatch(input, ADDRESS_PATTERN, RegexOptions.Compiled);

    // //[Benchmark]
    // //[ArgumentsSource(nameof(ValuesForInput))]
    // public bool UseStaticRegexWithUncached(string input)
    // {
    //     var defaultSize = Regex.CacheSize;
    //     Regex.CacheSize = 0;
    //     var isMatch = Regex.IsMatch(input, ADDRESS_PATTERN, RegexOptions.Compiled);
    //     Regex.CacheSize = defaultSize;

    //     return isMatch;
    // }

#if NET7_0_OR_GREATER

    [Benchmark]
    [ArgumentsSource(nameof(ValuesForInput))]
    public bool WhenUsingGeneratedRegex(string input) =>
        GeneratedRegex.IsMatch(input);

    private static readonly Regex GeneratedRegex = MyRegex();

    [GeneratedRegex(ADDRESS_PATTERN, RegexOptions.Compiled)]
    private static partial Regex MyRegex();

#endif
}
