using System.Runtime.CompilerServices;
using System.Text;

namespace Examples.Text.Tests.Encoding;

using Encoding = System.Text.Encoding;

/// <summary>
/// Tests Shift_JIS Encoding generation with some names.
/// </summary>
/// <remarks>
/// It seems there is no need to install <c>System.Text.Encoding.CodePages</c>.
/// </remarks>
public class ShiftJISEncodingTests
{
    [ModuleInitializer]
    internal static void Initialize()
    {
        // Declaration to use Shift_JIS encoding.
        Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
    }

    public static readonly Encoding Encoding932 = Encoding.GetEncoding(932);

    public class GetEncodingMethod
    {
        [Theory]
        [InlineData("Shift_JIS")]
        [InlineData("SHIFT_JIS")]
        [InlineData("shift_jis")]
        [InlineData("shift_jiS")]
        [InlineData("Shift-JIS")]
        [InlineData("shift-jis")]
        [InlineData("sjis")]
        public void When_ValidNames_Then_ReturnsShiftJISEncoding(string name)
        {
            Assert.Equal(Encoding932, Encoding.GetEncoding(name));
        }
        // spell-checker: words sjis

        [Theory]
        [InlineData("s-jis")]
        [InlineData("windows31j")]
        [InlineData("MS932")]
        [InlineData("CP932")]
        [InlineData("shift jis")]
        public void When_InvalidNames_Then_ThrowsArgumentException(string name)
        {
            Assert.Throws<ArgumentException>(() => Encoding.GetEncoding(name));
        }
    }

    [Fact]
    public void When_ProveWhereCodePagesIsRegistered_Then_ReturnsAssemblyLocation()
    {
        // It appears that registering `System.Text.Encoding.CodePages`
        // is no longer necessary in .NET 10, so I will verify this.

        var enc = Encoding.GetEncoding("Shift_JIS");
        Assert.NotNull(enc);

        var console = TestContext.Current.TestOutputHelper;
        console?.WriteLine($"Shift_JIS provider class: {enc.GetType().FullName}");
        console?.WriteLine($"Shift_JIS provider DLL  : {enc.GetType().Assembly.Location}");
    }
}
