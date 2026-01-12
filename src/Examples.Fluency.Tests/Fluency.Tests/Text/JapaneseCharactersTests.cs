namespace Examples.Fluency.Text.Tests;

/// <summary>
/// Tests <see cref="JapaneseCharacters" /> methods.
/// </summary>
public class JapaneseCharactersTests(ITestOutputHelper output)
{

    [Fact]
    public void When_CallingIsHiragana_WithValidValues_Then_ReturnsTrue()
    {
        foreach (var input in JapaneseCharacters.EnumerateHiragana())
        {
            output.Write($"{input}");
            Assert.True(JapaneseCharacters.IsHiragana(input));
        }
        output.WriteLine(".");
    }

    [Fact]
    public void When_CallingIsHiragana_WithInvalidValues_Then_ReturnsFalse()
    {
        // value < \u3040.
        const char ideographicHalfFillSpace = '\u303F';
        output.WriteLine($"{ideographicHalfFillSpace}");

        Assert.False(JapaneseCharacters.IsFullWidthKatakana(ideographicHalfFillSpace));

        // \u309F < value.
        const char katakanaHiraganaDoubleHyphen = '\u30A0';
        output.WriteLine($"{katakanaHiraganaDoubleHyphen}");

        Assert.False(JapaneseCharacters.IsHiragana(katakanaHiraganaDoubleHyphen));
    }

    [Fact]
    public void When_CallingIsFullWidthKatakana_WithValidValues_Then_ReturnsTrue()
    {
        foreach (var input in JapaneseCharacters.EnumerateFullWidthKatakana())
        {
            output.Write($"{input}");

            Assert.True(JapaneseCharacters.IsFullWidthKatakana(input));
        }
        output.WriteLine(".");
    }

    [Fact]
    public void WhenCallingIsFullWidthKatakana_WithInvalidValues_ReturnsFalse()
    {
        // value < \u30A0 (HIRAGANA DIGRAPH YORI).
        const char hiraganaDigraphYori = '\u309F';
        output.WriteLine($"{hiraganaDigraphYori}");

        Assert.False(JapaneseCharacters.IsFullWidthKatakana(hiraganaDigraphYori));

        // \u30FF (Bopomofo - 1) < value. 
        const char bopomofoUndefined = '\u3100';
        output.WriteLine($"{bopomofoUndefined}");

        Assert.False(JapaneseCharacters.IsFullWidthKatakana(bopomofoUndefined));
    }

    [Fact]
    public void WhenCallingIsHalfWidthKatakana_WithValidValues_ReturnsTrue()
    {
        foreach (var input in JapaneseCharacters.EnumerateHalfWidthKatakana())
        {
            output.Write($"{input}");
            Assert.True(JapaneseCharacters.IsHalfWidthKatakana(input));
        }
        output.WriteLine(".");
    }

    [Fact]
    public void WhenCallingIsHalfWidthKatakana_WithInvalidValues_ReturnsFalse()
    {
        // value < \uFF60.
        const char fullWidthRightWhiteParenthesis = '\uFF60';
        output.WriteLine($"{fullWidthRightWhiteParenthesis}");
        Assert.False(JapaneseCharacters.IsHalfWidthKatakana(fullWidthRightWhiteParenthesis));

        // \uFF9F < value.
        const char halfWidthHangulFiller = '\uFFA0';
        output.WriteLine($"{halfWidthHangulFiller}");
        Assert.False(JapaneseCharacters.IsHalfWidthKatakana(halfWidthHangulFiller));
    }

}
