using System.Text;

namespace Examples.Fluency.Tests;

public class EncodingExtensionsTests
{
    [Fact]
    public void When_CallingRemovePreamble_WithUTF8Encoding_Then_ReturnsAsExpected()
    {
        // Create a UTF8 string with BOM added at the beginning
        const string baseString = "TEST文字列😄";
        byte[] original;
        using (var stream = new MemoryStream())
        {
            stream.Write(Encoding.UTF8.GetPreamble());
            stream.Write(Encoding.UTF8.GetBytes(baseString));
            stream.Flush();

            original = stream.ToArray();
        }

        // The default is UTF8, so the result will be the same whether you specify it or not.
        var actual1 = original.RemovePreamble();
        var actual2 = original.RemovePreamble(Encoding.UTF8);

        // Removed BOM.
        Assert.Equal(Encoding.UTF8.GetBytes(baseString), actual1);
        Assert.Equal(Encoding.UTF8.GetBytes(baseString), actual2);

        // Encoding.UTF8.GetString() is auto remove?
        {
            var decoded = Encoding.UTF8.GetString(original);
            var actual = Encoding.UTF8.GetBytes(decoded);

            var bom = Encoding.UTF8.GetPreamble();

            Assert.Equal(bom, actual[..bom.Length]); // not removed.
            Assert.Equal(Encoding.UTF8.GetBytes(baseString), actual[bom.Length..]);
        }

        // Binary reader obediently reads from the preamble.
        using (var stream = new MemoryStream(original))
        using (var reader = new BinaryReader(stream, Encoding.UTF8))
        {
            var actual = reader.ReadBytes((int)stream.Length);

            var bom = Encoding.UTF8.GetPreamble();

            Assert.Equal(bom, actual[..bom.Length]); // not removed.
            Assert.Equal(Encoding.UTF8.GetBytes(baseString), actual[bom.Length..]);
        }

        // Binary reader obediently reads from the preamble.
        using (var stream = new MemoryStream(original))
        using (var reader = new StreamReader(stream, Encoding.UTF8))
        {
            Span<char> buffer = stackalloc char[(int)stream.Length];
            reader.ReadBlock(buffer);

            Assert.Equal(baseString[0], buffer[0]); // removed.

            var actual = buffer.ToString();
            Assert.NotEqual(baseString, actual); // null-terminated.
            Assert.Equal(baseString, actual.TrimEnd('\0'));
        }

        // UTF-8 encoding BOM is...
        Assert.Equal(new byte[] { 0xEF, 0xBB, 0xBF }, Encoding.UTF8.GetPreamble());
    }

    [Fact]
    public void When_CallingRemovePreamble_WithUTF16Encoding_Then_ReturnsAsExpected()
    {
        // Create a UTF8 string with BOM added at the beginning

        var utf16 = Encoding.GetEncoding("UTF-16");

        const string baseString = "TEST文字列😄";
        byte[] original;
        using (var stream = new MemoryStream())
        {
            stream.Write(utf16.GetPreamble());
            stream.Write(utf16.GetBytes(baseString));
            stream.Flush();

            original = stream.ToArray();
        }

        // The default is UTF8, so the result will be the same whether you specify it or not.
        var actual = original.RemovePreamble(utf16);

        // Removed BOM.
        Assert.Equal(utf16.GetBytes(baseString), actual);

        // UTF-16 Little Endian encoding is...
        Assert.Equal(new byte[] { 0xFF, 0xFE }, utf16.GetPreamble());
    }
}
