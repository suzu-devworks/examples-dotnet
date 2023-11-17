using System.Text;
using Examples.Fluency;

namespace Examples.Tests.Fluency;

public class EncodingExtensionsTests
{
    [Fact]
    public void WhenCallingRemovePreamble_WithUTF8Encoding_ReturnsExpected()
    {
        // ### Arrange ###
        // Create a UTF8 string with BOM added at the beginning
        const string BASE_STRING = "TEST文字列😄";
        byte[] original;
        using (var stream = new MemoryStream())
        {
            stream.Write(Encoding.UTF8.GetPreamble());
            stream.Write(Encoding.UTF8.GetBytes(BASE_STRING));
            stream.Flush();

            original = stream.ToArray();
        }

        // ### Act ###
        // The default is UTF8, so the result will be the same whether you specify it or not.
        var actual1 = original.RemovePreamble();
        var actual2 = original.RemovePreamble(Encoding.UTF8);

        // ### Assert ###
        // Removed BOM.
        actual1.Is(Encoding.UTF8.GetBytes(BASE_STRING));
        actual2.Is(Encoding.UTF8.GetBytes(BASE_STRING));

        // Encoding.UTF8.GetString() is auto remove?
        {
            var decoded = Encoding.UTF8.GetString(original);
            var actual = Encoding.UTF8.GetBytes(decoded);

            var bom = Encoding.UTF8.GetPreamble();
            actual[..bom.Length].Is(bom); // not removed.
            actual[bom.Length..].Is(Encoding.UTF8.GetBytes(BASE_STRING));
        }

        // Binary reader obediently reads from the preamble.
        using (var stream = new MemoryStream(original))
        using (var reader = new BinaryReader(stream, Encoding.UTF8))
        {
            var actual = reader.ReadBytes((int)stream.Length);

            var bom = Encoding.UTF8.GetPreamble();
            actual[..bom.Length].Is(bom); // not removed.
            actual[bom.Length..].Is(Encoding.UTF8.GetBytes(BASE_STRING));
        }

        // StreamReader is auto remove?
        using (var stream = new MemoryStream(original))
        using (var reader = new StreamReader(stream, Encoding.UTF8))
        {
            Span<char> buffer = stackalloc char[(int)stream.Length];
            reader.ReadBlock(buffer);
            buffer[0].Is(BASE_STRING[0]); // removed.
            var actual = buffer.ToString();
            actual.IsNot(BASE_STRING); // null-terminated.
            actual.TrimEnd('\0').Is(BASE_STRING);
        }

        // UTF-8 encoding BOM is...
        Encoding.UTF8.GetPreamble().Is(new byte[] { 0xEF, 0xBB, 0xBF });
    }

    [Fact]
    public void WhenCallingRemovePreamble_WithUTF16Encoding_ReturnsExpected()
    {
        // ### Arrange ###
        // Create a UTF8 string with BOM added at the beginning

        var utf16 = Encoding.GetEncoding("UTF-16");

        const string BASE_STRING = "TEST文字列😄";
        byte[] original;
        using (var stream = new MemoryStream())
        {
            stream.Write(utf16.GetPreamble());
            stream.Write(utf16.GetBytes(BASE_STRING));
            stream.Flush();

            original = stream.ToArray();
        }

        // ### Act ###
        // The default is UTF8, so the result will be the same whether you specify it or not.
        var actual = original.RemovePreamble(utf16);

        // ### Assert ###
        // Removed BOM.
        actual.Is(utf16.GetBytes(BASE_STRING));

        // UTF-16 Little Endian encoding is...
        utf16.GetPreamble().Is(new byte[] { 0xFF, 0xFE });
    }
}
