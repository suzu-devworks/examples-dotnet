using System.Diagnostics;

namespace Examples.Text.Tests;

/// <summary>
/// Tests <see cref="JapaneseCharacters" /> methods.
/// </summary>
public class JapaneseCharactersTests
{

    [Fact]
    public void WhenCallingIsHiragana_WithValidValues_ReturnsTrue()
    {
        foreach (var input in JapaneseCharacters.EnumerateHiragana())
        {
            DebugWrite(input);
            JapaneseCharacters.IsHiragana(input).IsTrue();
        }
        DebugWriteLine('.');
    }

    [Fact]
    public void WhenCallingIsHiragana_WithInvalidValues_ReturnsFalse()
    {
        // value < \u3040.
        const char ideographicHalfFillSpace = '\u303F';
        Debug.WriteLine(ideographicHalfFillSpace);
        JapaneseCharacters.IsFullWidthKatakana(ideographicHalfFillSpace).IsFalse();

        // \u309F < value.
        const char katakanaHiraganaDoubleHyphen = '\u30A0';
        DebugWriteLine(katakanaHiraganaDoubleHyphen);
        JapaneseCharacters.IsHiragana(katakanaHiraganaDoubleHyphen).IsFalse();
    }

    [Fact]
    public void WhenCallingIsFullWidthKatakana_WithValidValues_ReturnsTrue()
    {
        foreach (var input in JapaneseCharacters.EnumerateFullWidthKatakana())
        {
            DebugWrite(input);
            JapaneseCharacters.IsFullWidthKatakana(input).IsTrue();
        }
        DebugWriteLine('.');
    }

    [Fact]
    public void WhenCallingIsFullWidthKatakana_WithInvalidValues_ReturnsFalse()
    {
        // value < \u30A0.
        const char hiraganaDigraphYori = '\u309F';
        DebugWriteLine(hiraganaDigraphYori);
        JapaneseCharacters.IsFullWidthKatakana(hiraganaDigraphYori).IsFalse();

        // \u30FF < value.
        const char bopomofoUndefined = '\u3100';
        DebugWriteLine(bopomofoUndefined);
        JapaneseCharacters.IsFullWidthKatakana(bopomofoUndefined).IsFalse();
    }

    [Fact]
    public void WhenCallingIsHalfWidthKatakana_WithValidValues_ReturnsTrue()
    {
        foreach (var input in JapaneseCharacters.EnumerateHalfWidthKatakana())
        {
            DebugWrite(input);
            JapaneseCharacters.IsHalfWidthKatakana(input).IsTrue();
        }
        DebugWriteLine('.');
    }

    [Fact]
    public void WhenCallingIsHalfWidthKatakana_WithInvalidValues_ReturnsFalse()
    {
        // value < \uFF60.
        const char fullWidthRightWhiteParenthesis = '\uFF60';
        DebugWriteLine(fullWidthRightWhiteParenthesis);
        JapaneseCharacters.IsHalfWidthKatakana(fullWidthRightWhiteParenthesis).IsFalse();

        // \uFF9F < value.
        const char halfWidthHangulFiller = '\uFFA0';
        DebugWriteLine(halfWidthHangulFiller);
        JapaneseCharacters.IsHalfWidthKatakana(halfWidthHangulFiller).IsFalse();
    }

#if DEBUG

    private static void DebugWrite(char input) => Debug.Write(input);
    private static void DebugWriteLine(char input) => Debug.WriteLine(input);

#else

    private static void DebugWrite(char _) { }
    private static void DebugWriteLine(char _) { }

#endif

}
