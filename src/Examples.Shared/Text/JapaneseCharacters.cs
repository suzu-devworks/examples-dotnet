using System;
using System.Collections.Generic;
using System.Linq;

namespace Examples.Text;

/// <summary>
/// A utility for handling Japanese characters.
/// </summary>
public static class JapaneseCharacters
{
    /// <summary>
    /// Indicates whether the specified Unicode character is Japanese Hiragana.
    /// </summary>
    /// <param name="char">The character to test.</param>
    /// <returns><c>true</c> if the character is Japanese Hiragana;
    ///     otherwise <c>false</c>.</returns>
    public static bool IsHiragana(char @char)
        => @char is >= '\u3040' and <= '\u309F';

    /// <summary>
    /// Indicates whether the specified Unicode character is Japanese Full width Katakana.
    /// </summary>
    /// <param name="char">The character to test.</param>
    /// <returns><c>true</c> if the character is Japanese Full width Katakana;
    ///     otherwise <c>false</c>.</returns>
    public static bool IsFullWidthKatakana(char @char)
        => @char is >= '\u30A0' and <= '\u30FF';

    /// <summary>
    /// Indicates whether the specified Unicode character is Japanese Half width Katakana.
    /// </summary>
    /// <param name="char">The character to test.</param>
    /// <returns><c>true</c> if the character is Japanese Half width Katakana;
    ///     otherwise <c>false</c>.</returns>
    public static bool IsHalfWidthKatakana(char @char)
        => @char is >= '\uFF61' and <= '\uFF9F';

    /// <summary>
    /// Returns an enumerable collection of Japanese Hiragana.
    /// </summary>
    /// <returns>An enumerable collection of Japanese Hiragana.</returns>
    public static IEnumerable<char> EnumerateHiragana()
        => EnumerateCharacters('\u3041', '\u309F');

    /// <summary>
    /// Returns an enumerable collection of Japanese Full width Katakana.
    /// </summary>
    /// <returns>An enumerable collection of Japanese Full width Katakana.</returns>
    public static IEnumerable<char> EnumerateFullWidthKatakana()
        => EnumerateCharacters('\u30A0', '\u30FF');

    /// <summary>
    /// Returns an enumerable collection of Japanese Half width Katakana.
    /// </summary>
    /// <returns>An enumerable collection of Japanese Half width Katakana.</returns>
    public static IEnumerable<char> EnumerateHalfWidthKatakana()
        => EnumerateCharacters('\uFF61', '\uFF9F');

    private static IEnumerable<char> EnumerateCharacters(char start, char end)
    {
        foreach (var c in Enumerable.Range((int)start, (int)end - (int)start + 1))
        {
            yield return Convert.ToChar(c);
        }
    }
}
