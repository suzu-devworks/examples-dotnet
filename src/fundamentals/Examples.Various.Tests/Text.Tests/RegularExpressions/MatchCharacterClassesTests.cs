using System.Text;
using System.Text.RegularExpressions;

namespace Examples.Text.Tests._.System.Text.RegularExpressions;

#pragma warning disable SYSLIB1045 // Use 'GeneratedRegexAttribute'

/// <summary>
/// Tests to match with character class.
/// </summary>
/// <seealso href="https://docs.microsoft.com/ja-jp/dotnet/standard/base-types/character-classes-in-regular-expressions" />
public class MatchCharacterClassesTests
{
    [Theory]
    [MemberData(nameof(ValidCharacterForIsMatchUnicodeCategory))]
    public void When_CharacterClass_Then_ReturnsMatch(string pattern, string input, string name)
    {
        Assert.True(Regex.IsMatch(input, pattern), name);
    }

    public static readonly TheoryData<string, string, string> ValidCharacterForIsMatchUnicodeCategory = new()
    {
        // spell-checker: disable
        // Character Categories.
        { @"^\p{Lu}+$", "LETTER", "Letter, Uppercase." },
        { @"^\p{Ll}+$", "letter", "Letter, Lowercase." },
        { @"^\p{L}+$", "Letter", "All letter characters. (Lu, Ll, Lt, Lm, and Lo)." },
        { @"^\p{Nd}+$", "0123456789٠١٢٣", "Number, Decimal Digit." },
        { @"^\p{Nl}+$", "ⅠⅡⅢⅣⅰⅱⅲⅳ", "Number, Letter." },
        { @"^\p{No}+$", "①❷⑶⒋㈤", "Number, Other." },
        { @"^\p{P}+$", "(){}[]", "All punctuation characters. (Pc, Pd, Ps, Pe, Pi, Pf, and Po)." },
        { @"^\p{Sc}+$", "¥$€£", "Symbol, Currency." },
        { @"^\p{Sm}+$", "+<=>~¬±×÷→∂∆√∞∧∨∩∪∫∵", "Symbol, Math." },
        { @"^\p{S}+$", "©★☞♬✔㊑㋋㌫", "All symbols. (Sm, Sc, Sk, and So)." },
        { @"^\p{Z}+$", " 　", "All separator characters. (Zs, Zl, and Zp)." },
        { @"^\p{C}+$", "\u0000\u0009\u000A\u000D", "All control characters. (Cc, Cf, Cs, Co, and Cn)." },

        // Named block.
        { @"^\p{IsHiragana}+$", "あいうえおゑゐん", "IsHiragana(3040 ～ 309F)" },
        { @"^\p{IsKatakana}+$", "カタカナ", "IsKatakana(30A0 ～ 30FF)" },
        { @"^\p{IsHalfwidthandFullwidthForms}+$", "ﾊﾝｶｸｶﾅ", "IsHalfwidthandFullwidthForms(FF00 ～ FFEF)" },
        // spell-checker: enable
    };

    [Fact]
    public void When_CharacterClassSubtract_Then_ExcludedCharactersNotMatch()
    {
        // Exclude 'A' from Letter, Uppercase.
        var pattern = @"^[\p{Lu}-[A]]+$";

        // does not include 'A' ... is Match.
        Assert.Matches(pattern, "BCDEFG");
        // spell-checker: words BCDEFG

        // include 'A' ... is Not Match.
        Assert.DoesNotMatch(pattern, "ABCDEF");
    }

    [Fact]
    public void When_HalfKanaRange_Then_ReturnsMatch()
    {
        var input = Enumerable.Range('\uFF61', '\uFF9F' - '\uFF61' + 1)
            .Where(c => !char.IsSurrogate((char)c))
            .Select(Convert.ToChar)
            .Aggregate(new StringBuilder(), (builder, c) => builder.Append(c))
            .ToString();

        Assert.True(Regex.IsMatch(input, @"^[\uFF61-\uFF9F]+$", RegexOptions.None));
        Assert.True(Regex.IsMatch(input, @"^[｡-ﾟ]+$", RegexOptions.None));
    }
}
