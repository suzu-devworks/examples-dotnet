using Examples.Fluency.Text;

namespace Examples.Fluency.Tests.Text;

/// <summary>
/// Tests <see cref="JapaneseCharacters" /> methods.
/// </summary>
public class JapaneseCharactersTests
{
    public class IsHiraganaMethod
    {
        [Fact]
        public void When_EnumerateHiragana_Then_ReturnsTrue()
        {
            foreach (var input in JapaneseCharacters.EnumerateHiragana())
            {
                Assert.True(JapaneseCharacters.IsHiragana(input));
            }
        }

        [Theory]
        [InlineData('\u3039')] // value < \u3040.
        [InlineData('\u3100')] // \u309F < value.
        public void When_WithInvalidValues_Then_ReturnsFalse(char input)
        {
            Assert.False(JapaneseCharacters.IsHiragana(input));
        }
    }

    public class IsFullWidthKatakanaMethod
    {
        [Fact]
        public void When_EnumerateFullWidthKatakana_Then_ReturnsTrue()
        {
            foreach (var input in JapaneseCharacters.EnumerateFullWidthKatakana())
            {
                Assert.True(JapaneseCharacters.IsFullWidthKatakana(input));
            }
        }

        [Theory]
        [InlineData('\u309F')] // value < \u30A0 (HIRAGANA DIGRAPH YORI).
        [InlineData('\u3100')] // \u30FF (Bopomofo - 1) < value.
        public void When_InvalidValues_Then_ReturnsFalse(char input)
        {
            Assert.False(JapaneseCharacters.IsFullWidthKatakana(input));
        }
    }

    public class IsHalfWidthKatakanaMethod
    {
        [Fact]
        public void When_EnumerateHalfWidthKatakana_Then_ReturnsTrue()
        {
            foreach (var input in JapaneseCharacters.EnumerateHalfWidthKatakana())
            {
                Assert.True(JapaneseCharacters.IsHalfWidthKatakana(input));
            }
        }

        [Theory]
        [InlineData('\uFF60')] // value < \uFF60.
        [InlineData('\uFFA0')] // \uFF9F < value.
        public void When_InvalidValues_Then_ReturnsFalse(char input)
        {
            Assert.False(JapaneseCharacters.IsHalfWidthKatakana(input));
        }
    }
}
