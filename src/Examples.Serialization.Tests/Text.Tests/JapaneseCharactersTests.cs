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
        foreach (var input in CharacterGenerator.EnumerateHiragana())
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
        const char IDEOGRAPHIC_HALF_FILL_SPACE = '\u303F';
        Debug.WriteLine(IDEOGRAPHIC_HALF_FILL_SPACE);
        JapaneseCharacters.IsFullWidthKatakana(IDEOGRAPHIC_HALF_FILL_SPACE).IsFalse();

        // \u309F < value.
        const char KATAKANA_HIRAGANA_DOUBLE_HYPHEN = '\u30A0';
        DebugWriteLine(KATAKANA_HIRAGANA_DOUBLE_HYPHEN);
        JapaneseCharacters.IsHiragana(KATAKANA_HIRAGANA_DOUBLE_HYPHEN).IsFalse();
    }

    [Fact]
    public void WhenCallingIsFullWidthKatakana_WithValidValues_ReturnsTrue()
    {
        foreach (var input in CharacterGenerator.EnumerateFullWidthKatakana())
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
        const char HIRAGANA_DIGRAPH_YORI = '\u309F';
        DebugWriteLine(HIRAGANA_DIGRAPH_YORI);
        JapaneseCharacters.IsFullWidthKatakana(HIRAGANA_DIGRAPH_YORI).IsFalse();

        // \u30FF < value.
        const char BOPOMOFO_UNDEFINED = '\u3100';
        DebugWriteLine(BOPOMOFO_UNDEFINED);
        JapaneseCharacters.IsFullWidthKatakana(BOPOMOFO_UNDEFINED).IsFalse();
    }

    [Fact]
    public void WhenCallingIsHalfWidthKatakana_WithValidValues_ReturnsTrue()
    {
        foreach (var input in CharacterGenerator.EnumerateHalfWidthKatakana())
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
        const char FULL_WIDTH_RIGHT_WHITE_PARENTHESIS = '\uFF60';
        DebugWriteLine(FULL_WIDTH_RIGHT_WHITE_PARENTHESIS);
        JapaneseCharacters.IsHalfWidthKatakana(FULL_WIDTH_RIGHT_WHITE_PARENTHESIS).IsFalse();

        // \uFF9F < value.
        const char HALF_WIDTH_HANGUL_FILLER = '\uFFA0';
        DebugWriteLine(HALF_WIDTH_HANGUL_FILLER);
        JapaneseCharacters.IsHalfWidthKatakana(HALF_WIDTH_HANGUL_FILLER).IsFalse();
    }

#if DEBUG
    private static void DebugWrite(char input) => Debug.Write(input);
    private static void DebugWriteLine(char input) => Debug.WriteLine(input);

#else
    private static void DebugWrite(char _) { }
    private static void DebugWriteLine(char _) { }

#endif

}
