namespace Examples.Serialization.Text.Tests;

internal static class CharacterGenerator
{
    public static IEnumerable<char> EnumerateHiragana()
        => EnumerateCharacters('\u3041', '\u309F');

    public static IEnumerable<char> EnumerateFullWidthKatakana()
        => EnumerateCharacters('\u30A0', '\u30FF');

    public static IEnumerable<char> EnumerateHalfWidthKatakana()
        => EnumerateCharacters('\uFF61', '\uFF9F');

    private static IEnumerable<char> EnumerateCharacters(char start, char end)
    {
        foreach (var c in Enumerable.Range(start, end - start + 1))
        {
            yield return Convert.ToChar(c);
        }
    }

}
