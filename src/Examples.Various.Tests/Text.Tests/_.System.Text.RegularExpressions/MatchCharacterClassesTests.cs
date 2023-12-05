using System.Text;
using System.Text.RegularExpressions;
using Examples.Text;

namespace Examples.Text.Tests._.System.Text.RegularExpressions;

/// <summary>
/// Tests to match with character class.
/// </summary>
/// <seealso href="https://docs.microsoft.com/ja-jp/dotnet/standard/base-types/character-classes-in-regular-expressions" />
public class MatchCharacterClassesTests
{

    [Theory]
    [MemberData(nameof(ValidDataForIsMatchUnicodeCategory))]
    public void WhenCallingIsMatch_WithSomeCharacterClass_ReturnsTrue(string pattern, string input, string name)
    {
        Regex.IsMatch(input, pattern).IsTrue(name);
    }

    public static readonly TheoryData<string, string, string> ValidDataForIsMatchUnicodeCategory = new()
    {
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
    };


    [Fact]
    public void WhenCallingIsMatch_WithCharacterClassSubtract_ReturnsAsExpected()
    {
        var pattern = @"^[\p{Lu}-[A]]+$";

        // does not include 'A'.
        Regex.IsMatch("BCDEFG", pattern).IsTrue();

        // include 'A'
        Regex.IsMatch("ABCDEF", pattern).IsFalse();
    }


    [Fact]
    public void WhenCallingIsMatch_WithHalfKanaRange_ReturnsTrue()
    {
        var input = GenerateAllHalfKanaString();

        var patternOfUnicodeCodePoint = @"^[\uFF61-\uFF9F]+$";
        Regex.IsMatch(input, patternOfUnicodeCodePoint).IsTrue();

        // specify character.
        var patternOfCharacter = @"^[｡-ﾟ]+$";
        Regex.IsMatch(input, patternOfCharacter).IsTrue();
    }


    public static string GenerateAllHalfKanaString()
    {
        var result = JapaneseCharacters.EnumerateHalfWidthKatakana()
            .Aggregate(
                new StringBuilder(),
                (builder, num) => builder.Append((char)num),
                builder => builder.ToString());

        return result;
    }

}
