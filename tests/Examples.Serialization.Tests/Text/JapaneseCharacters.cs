namespace Examples.Text;

/// <summary>
/// A utility for handling Japanese characters.
/// </summary>
/// <seealso href="https://stackoverflow.com/questions/19899554/unicode-range-for-japanese" />
/// <seealso href="https://www.unicode.org/charts/PDF/U3040.pdf" />
/// <seealso href="https://www.unicode.org/charts/PDF/U30A0.pdf" />
/// <seealso href="https://www.unicode.org/charts/PDF/UFF00.pdf" />
public static class JapaneseCharacters
{
    /// <summary>
    /// Indicates whether the specified Unicode character is Japanese Hiragana.
    /// </summary>
    /// <param name="char">The character to test.</param>
    /// <returns><see cref="true" /> if the character is Japanese Hiragana;
    ///     otherwise <see cref="false" />.</returns>
    public static bool IsHiragana(char @char)
        => @char is >= '\u3040' and <= '\u309F';

    /// <summary>
    /// Indicates whether the specified Unicode character is Japanese Full width Katakana.
    /// </summary>
    /// <param name="char">The character to test.</param>
    /// <returns><see cref="true" /> if the character is Japanese Full width Katakana;
    ///     otherwise <see cref="false" />.</returns>
    public static bool IsFullWidthKatakana(char @char)
        => @char is >= '\u30A0' and <= '\u30FF';

    /// <summary>
    /// Indicates whether the specified Unicode character is Japanese Half width Katakana.
    /// </summary>
    /// <param name="char">The character to test.</param>
    /// <returns><see cref="true" /> if the character is Japanese Half width Katakana;
    ///     otherwise <see cref="false" />.</returns>
    public static bool IsHalfWidthKatakana(char @char)
        => @char is >= '\uFF61' and <= '\uFF9F';

}
