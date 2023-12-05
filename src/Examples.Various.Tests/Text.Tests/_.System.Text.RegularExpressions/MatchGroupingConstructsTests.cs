using System.Text.RegularExpressions;

namespace Examples.Text.Tests._.System.Text.RegularExpressions;

/// <summary>
/// Tests to match with Grouping constructs.
/// </summary>
/// <seealso href="https://docs.microsoft.com/ja-jp/dotnet/standard/base-types/grouping-constructs-in-regular-expressions" />
public class MatchGroupingConstructsTests
{
    [Fact]
    public void WhenExpression_WithSubExpressions()
    {
        // (subexpression)

        var pattern = @"(\w+)\s(\1)\W";
        var input = "He said that that was the the correct answer.";

        var matches = Regex.Matches(input, pattern, RegexOptions.IgnoreCase);

        matches.Count.Is(2);

        matches[0].Groups.Count.Is(3);
        matches[0].Groups[0].Value.Is("that that ");
        matches[0].Groups[1].Value.Is("that");
        matches[0].Groups[1].Index.Is(8);
        matches[0].Groups[2].Index.Is(13);

        matches[1].Groups.Count.Is(3);
        matches[1].Groups[0].Value.Is("the the ");
        matches[1].Groups[1].Value.Is("the");
        matches[1].Groups[1].Index.Is(22);
        matches[1].Groups[2].Index.Is(26);

        return;
    }

    [Fact]
    public void WhenExpression_WithNamedSubExpressions()
    {
        // (?<name>subexpression)

        var pattern = @"(?<duplicateWord>\w+)\s\k<duplicateWord>\W(?<nextWord>\w+)";
        var input = "He said that that was the the correct answer.";

        var matches = Regex.Matches(input, pattern, RegexOptions.IgnoreCase);

        matches.Count.Is(2);

        matches[0].Groups.Count.Is(3);
        matches[0].Groups[0].Value.Is("that that was");
        matches[0].Groups["duplicateWord"].Value.Is("that");
        matches[0].Groups["duplicateWord"].Index.Is(8);
        matches[0].Groups["nextWord"].Value.Is("was");
        matches[0].Groups["nextWord"].Index.Is(18);

        matches[1].Groups.Count.Is(3);
        matches[1].Groups[0].Value.Is("the the correct");
        matches[1].Groups["duplicateWord"].Value.Is("the");
        matches[1].Groups["duplicateWord"].Index.Is(22);
        matches[1].Groups["nextWord"].Value.Is("correct");
        matches[1].Groups["nextWord"].Index.Is(30);

        return;
    }

    [Fact]
    public void WhenExpression_WithBalancingGroups()
    {
        // (?<name1-name2>subexpression)

        var pattern = @"^[^<>]*(((?'Open'<)[^<>]*)+((?'Close-Open'>)[^<>]*)+)*(?(Open)(?!))$";
        var input = "<abc><mno<xyz>>";

        var match = Regex.Match(input, pattern, RegexOptions.IgnoreCase);

        match.Groups.Count.Is(6);

        match.Groups[0].Captures.Count.Is(1);
        match.Groups[0].Captures[0].Value.Is("<abc><mno<xyz>>");

        //　(((?'Open'<)[^<>]*)+((?'Close-Open'>)[^<>]*)+)*
        match.Groups[1].Captures.Count.Is(2);
        match.Groups[1].Captures[0].Value.Is("<abc>");
        match.Groups[1].Captures[1].Value.Is("<mno<xyz>>");

        //　((?'Open'<)[^<>]*)+
        match.Groups[2].Captures.Count.Is(3);
        match.Groups[2].Captures[0].Value.Is("<abc");
        match.Groups[2].Captures[1].Value.Is("<mno");
        match.Groups[2].Captures[2].Value.Is("<xyz");

        //　((?'Close-Open'>)[^<>]*)+
        match.Groups[3].Captures.Count.Is(3);
        match.Groups[3].Captures[0].Value.Is(">");
        match.Groups[3].Captures[1].Value.Is(">");
        match.Groups[3].Captures[2].Value.Is(">");

        //　<Open> (?'Open'<)
        match.Groups[4].Captures.Count.Is(0);

        //　<Close> (?'Close-Open'>)
        match.Groups[5].Captures.Count.Is(3);
        match.Groups[5].Captures[0].Value.Is("abc");
        match.Groups[5].Captures[1].Value.Is("xyz");
        match.Groups[5].Captures[2].Value.Is("mno<xyz>");

        return;
    }

    [Fact]
    public void WhenExpression_WithNonCapturingGroups()
    {
        // (?:subexpression)

        var pattern = @"(?:\b(?:\w+)\W*)+\.";
        var input = "This is a short sentence.";

        var match = Regex.Match(input, pattern);

        match.Groups.Count.Is(1);
        match.Groups[0].Captures.Count.Is(1);
        match.Groups[0].Captures[0].Value.Is("This is a short sentence.");

        return;
    }

    [Fact]
    public void WhenExpression_WithGroupingOptions()
    {
        // (?imnsx-imnsx:subexpression)

        string pattern = @"\b(?ix: d \w+)\s";
        string input = "Dogs are decidedly good pets, and d .";

        var matches = Regex.Matches(input, pattern);

        matches.Count.Is(2);

        matches[0].Groups[0].Value.Is("Dogs ");
        matches[1].Groups[0].Value.Is("decidedly ");

        return;
    }


    [Fact]
    public void WhenExpression_WithZeroWidthPositiveLookaheadAssertions()
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

        matches[0].Success.IsTrue();
        matches[0].Value.Is("dog");

        matches[1].Success.IsFalse();
        matches[2].Success.IsFalse();
        matches[3].Success.IsFalse();

        matches[4].Success.IsTrue();
        matches[4].Value.Is("Sunday");

        return;
    }

    [Fact]
    public void WhenExpression_WithZeroWidthNegativeLookaheadAssertions()
    {
        // (?=subexpression)

        var pattern = @"\b(?!un)\w+\b";
        var input = "unite one unethical ethics use untie ultimate";

        var matches = Regex.Matches(input, pattern);

        matches.Count.Is(4);

        matches[0].Value.Is("one");
        matches[1].Value.Is("ethics");
        matches[2].Value.Is("use");
        matches[3].Value.Is("ultimate");

        return;
    }

    [Fact]
    public void WhenExpression_WithZeroWidthPositiveLookbehindAssertions()
    {
        // (?<=subexpression)

        var input = "2010 1999 1861 2140 2009";
        var pattern = @"(?<=\b20)\d{2}\b";

        var matches = Regex.Matches(input, pattern);

        matches.Count.Is(2);

        matches[0].Value.Is("10");
        matches[1].Value.Is("09");

        return;
    }

    [Fact]
    public void WhenExpression_WithZeroWidthNegativeLookbehindAssertions()
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

        matches[0].Success.IsTrue();
        matches[0].Value.Is("February 1, 2010");

        matches[1].Success.IsTrue();
        matches[1].Value.Is("February 3, 2010");

        matches[2].Success.IsFalse();
        matches[3].Success.IsFalse();

        matches[4].Success.IsTrue();
        matches[4].Value.Is("February 8, 2010");

        return;
    }

    [Fact]
    public void WhenExpression_WithAtomicGroups()
    {
        // (?>subexpression)

        string[] inputs = { "cccd.", "aaad", "aaaa" };

        var back = @"(\w)\1+.\b";
        var matches1 = inputs.Select(input => Regex.Match(input, back)).ToArray();

        matches1[0].Value.Is("cccd");
        matches1[1].Value.Is("aaad");
        matches1[2].Value.Is("aaaa");

        var noback = @"(?>(\w)\1+).\b";
        var matches2 = inputs.Select(input => Regex.Match(input, noback)).ToArray();

        matches2[0].Value.Is("cccd");
        matches2[1].Value.Is("aaad");
        matches2[2].Success.IsFalse();

        return;
    }

}
