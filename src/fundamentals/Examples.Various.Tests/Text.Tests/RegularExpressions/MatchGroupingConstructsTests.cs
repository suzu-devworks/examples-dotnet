using System.Text.RegularExpressions;

namespace Examples.Text.Tests.RegularExpressions;

/// <summary>
/// Tests to match with Grouping constructs.
/// </summary>
/// <seealso href="https://docs.microsoft.com/ja-jp/dotnet/standard/base-types/grouping-constructs-in-regular-expressions" />
public class MatchGroupingConstructsTests
{
    [Fact]
    public void When_MatchedSubexpressions_Then_MatchesExpectations()
    {
        // (subexpression)

        var pattern = @"(\w+)\s(\1)\W";
        var input = "He said that that was the the correct answer.";

        var matches = Regex.Matches(input, pattern, RegexOptions.IgnoreCase);

        Assert.Equal(2, matches.Count);

        Assert.Equal(3, matches[0].Groups.Count);
        Assert.Equal("that that ", matches[0].Groups[0].Value);
        Assert.Equal("that", matches[0].Groups[1].Value);
        Assert.Equal(8, matches[0].Groups[1].Index);
        Assert.Equal(13, matches[0].Groups[2].Index);

        Assert.Equal(3, matches[1].Groups.Count);
        Assert.Equal("the the ", matches[1].Groups[0].Value);
        Assert.Equal("the", matches[1].Groups[1].Value);
        Assert.Equal(22, matches[1].Groups[1].Index);
        Assert.Equal(26, matches[1].Groups[2].Index);
    }

    [Fact]
    public void When_NamedMatchedSubexpressions_Then_MatchesExpectations()
    {
        // (?<name>subexpression)

        var pattern = @"(?<duplicateWord>\w+)\s\k<duplicateWord>\W(?<nextWord>\w+)";
        var input = "He said that that was the the correct answer.";

        var matches = Regex.Matches(input, pattern, RegexOptions.IgnoreCase);

        Assert.Equal(2, matches.Count);

        Assert.Equal(3, matches[0].Groups.Count);
        Assert.Equal("that that was", matches[0].Groups[0].Value);
        Assert.Equal("that", matches[0].Groups["duplicateWord"].Value);
        Assert.Equal(8, matches[0].Groups["duplicateWord"].Index);
        Assert.Equal("was", matches[0].Groups["nextWord"].Value);
        Assert.Equal(18, matches[0].Groups["nextWord"].Index);

        Assert.Equal(3, matches[1].Groups.Count);
        Assert.Equal("the the correct", matches[1].Groups[0].Value);
        Assert.Equal("the", matches[1].Groups["duplicateWord"].Value);
        Assert.Equal(22, matches[1].Groups["duplicateWord"].Index);
        Assert.Equal("correct", matches[1].Groups["nextWord"].Value);
        Assert.Equal(30, matches[1].Groups["nextWord"].Index);
    }

    [Fact]
    public void When_BalancingGroupDefinitions_Then_MatchesExpectations()
    {
        // (?<name1-name2>subexpression)

        var pattern = @"^[^<>]*(((?'Open'<)[^<>]*)+((?'Close-Open'>)[^<>]*)+)*(?(Open)(?!))$";
        var input = "<abc><mno<xyz>>";

        var match = Regex.Match(input, pattern, RegexOptions.IgnoreCase);

        Assert.Equal(6, match.Groups.Count);

        Assert.Single(match.Groups[0].Captures);
        Assert.Equal("<abc><mno<xyz>>", match.Groups[0].Captures[0].Value);

        //　(((?'Open'<)[^<>]*)+((?'Close-Open'>)[^<>]*)+)*
        Assert.Equal(2, match.Groups[1].Captures.Count);
        Assert.Equal("<abc>", match.Groups[1].Captures[0].Value);
        Assert.Equal("<mno<xyz>>", match.Groups[1].Captures[1].Value);

        //　((?'Open'<)[^<>]*)+
        Assert.Equal(3, match.Groups[2].Captures.Count);
        Assert.Equal("<abc", match.Groups[2].Captures[0].Value);
        Assert.Equal("<mno", match.Groups[2].Captures[1].Value);
        Assert.Equal("<xyz", match.Groups[2].Captures[2].Value);

        //　((?'Close-Open'>)[^<>]*)+
        Assert.Equal(3, match.Groups[3].Captures.Count);
        Assert.Equal(">", match.Groups[3].Captures[0].Value);
        Assert.Equal(">", match.Groups[3].Captures[1].Value);
        Assert.Equal(">", match.Groups[3].Captures[2].Value);

        //　<Open> (?'Open'<)
        Assert.Empty(match.Groups[4].Captures);

        //　<Close> (?'Close-Open'>)
        Assert.Equal(3, match.Groups[5].Captures.Count);
        Assert.Equal("abc", match.Groups[5].Captures[0].Value);
        Assert.Equal("xyz", match.Groups[5].Captures[1].Value);
        Assert.Equal("mno<xyz>", match.Groups[5].Captures[2].Value);
    }

    [Fact]
    public void When_NonCapturingGroups_Then_MatchesExpectations()
    {
        // (?:subexpression)

        var pattern = @"(?:\b(?:\w+)\W*)+\.";
        var input = "This is a short sentence.";

        var match = Regex.Match(input, pattern);

        Assert.Single(match.Groups);
        Assert.Single(match.Groups[0].Captures);
        Assert.Equal("This is a short sentence.", match.Groups[0].Captures[0].Value);
    }

    [Fact]
    public void When_GroupOptions_Then_MatchesExpectations()
    {
        // (?imnsx-imnsx:subexpression)

        string pattern = @"\b(?ix: d \w+)\s";
        string input = "Dogs are decidedly good pets, and d .";

        var matches = Regex.Matches(input, pattern);

        Assert.Equal(2, matches.Count);

        Assert.Equal("Dogs ", matches[0].Groups[0].Value);
        Assert.Equal("decidedly ", matches[1].Groups[0].Value);
    }

    [Fact]
    public void When_ZeroWidthPositiveLookaheadAssertions_Then_MatchesExpectations()
    {
        // (?=subexpression)

        var pattern = @"\b\w+(?=\s[iI]s\b)";
        string[] inputs = {
                "The dog is a Malamute.",
                "The island has beautiful birds.",
                "The pitch missed home plate.",
                "Is this not dangerous?",
                "Sunday is a weekend day." };

        var matches = inputs.Select(input => Regex.Match(input, pattern)).ToArray();

        Assert.True(matches[0].Success);
        Assert.Equal("dog", matches[0].Value);

        Assert.False(matches[1].Success);
        Assert.False(matches[2].Success);
        Assert.False(matches[3].Success);

        Assert.True(matches[4].Success);
        Assert.Equal("Sunday", matches[4].Value);
    }

    [Fact]
    public void When_ZeroWidthNegativeLookaheadAssertions_Then_MatchesExpectations()
    {
        // (?=subexpression)

        var pattern = @"\b(?!un)\w+\b";
        var input = "unite one unethical ethics use untie ultimate";

        var matches = Regex.Matches(input, pattern);

        Assert.Equal(4, matches.Count);

        Assert.Equal("one", matches[0].Value);
        Assert.Equal("ethics", matches[1].Value);
        Assert.Equal("use", matches[2].Value);
        Assert.Equal("ultimate", matches[3].Value);
    }

    [Fact]
    public void When_ZeroWidthPositiveLookbehindAssertions_Then_MatchesExpectations()
    {
        // (?<=subexpression)

        var input = "2010 1999 1861 2140 2009";
        var pattern = @"(?<=\b20)\d{2}\b";

        var matches = Regex.Matches(input, pattern);

        Assert.Equal(2, matches.Count);

        Assert.Equal("10", matches[0].Value);
        Assert.Equal("09", matches[1].Value);
    }

    [Fact]
    public void When_ZeroWidthNegativeLookbehindAssertions_Then_MatchesExpectations()
    {
        // (?<!subexpression)

        var pattern = @"(?<!(Saturday|Sunday) )\b\w+ \d{1,2}, \d{4}\b";
        string[] inputs = {
                "Monday February 1, 2010",
                "Wednesday February 3, 2010",
                "Saturday February 6, 2010",
                "Sunday February 7, 2010",
                "Monday, February 8, 2010" };

        var matches = inputs.Select(input => Regex.Match(input, pattern)).ToArray();

        Assert.True(matches[0].Success);
        Assert.Equal("February 1, 2010", matches[0].Value);

        Assert.True(matches[1].Success);
        Assert.Equal("February 3, 2010", matches[1].Value);

        Assert.False(matches[2].Success);
        Assert.False(matches[3].Success);

        Assert.True(matches[4].Success);
        Assert.Equal("February 8, 2010", matches[4].Value);
    }

    [Fact]
    public void When_AtomicGroups_Then_MatchesExpectations()
    {
        // (?>subexpression)

        // spell-checker: disable-next-line
        string[] inputs = { "cccd.", "aaad", "aaaa" };

        var back = @"(\w)\1+.\b";
        var matches1 = inputs.Select(input => Regex.Match(input, back)).ToArray();

        // spell-checker: disable-next-line
        Assert.Equal("cccd", matches1[0].Value);
        // spell-checker: disable-next-line
        Assert.Equal("aaad", matches1[1].Value);
        // spell-checker: disable-next-line
        Assert.Equal("aaaa", matches1[2].Value);

        var noBack = @"(?>(\w)\1+).\b";
        var matches2 = inputs.Select(input => Regex.Match(input, noBack)).ToArray();

        // spell-checker: disable-next-line
        Assert.Equal("cccd", matches2[0].Value);
        // spell-checker: disable-next-line
        Assert.Equal("aaad", matches2[1].Value);
        Assert.False(matches2[2].Success);
    }
}
