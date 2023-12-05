using System.Text;

namespace Examples.Text.Tests._.System.Text;

/// <summary>
/// Tests Shift_JIS Encoding generation with some names.
/// </summary>
/// <remarks>
/// It seems there is no need to install <c>System.Text.Encoding.CodePages</c>.
/// </remarks>
public class ValidNameInShiftJISEncodingTests
{
    public ValidNameInShiftJISEncodingTests()
    {
        // Declaration to use Shift_JIS encoding.
        Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
    }

    [Theory]
    [InlineData("Shift_JIS")]
    [InlineData("SHIFT_JIS")]
    [InlineData("shift_jis")]
    [InlineData("shift_jiS")]
    [InlineData("Shift-JIS")]
    [InlineData("shift-jis")]
    [InlineData("sjis")]
    public void WhenCallingGetEncoding_WithValidName(string name)
    {
        var encoding932 = Encoding.GetEncoding(932);
        var actual = Encoding.GetEncoding(name);
        actual.Is(encoding932);
    }

    [Theory]
    [InlineData("s-jis")]
    [InlineData("windows31j")]
    [InlineData("MS932")]
    [InlineData("CP932")]
    [InlineData("shift jis")]
    public void WhenCallingGetEncoding_WithInvalidName_ThrowAnException(string name)
    {
        Assert.Throws<ArgumentException>(() => Encoding.GetEncoding(name));
    }

}
