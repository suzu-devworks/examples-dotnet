using System.Text;
using Examples.Fluency.Text;

namespace Examples.Fluency.Tests.Text;

public class EncodingExtensionsTests
{
    public class RemovePreambleMethod
    {
        [Fact]
        public void When_EncodingIsNull_Then_ReturnsValueWithoutBom()
        {
            var encoding = Encoding.UTF8;
            var bom = encoding.GetPreamble();
            byte[] original = BuildWithBom("TEST文字列😄", encoding);

            byte[] actual = original.RemovePreamble(null);

            Assert.Equal(bom, original[..bom.Length]); // exists.
            Assert.NotEqual(bom, actual[..bom.Length]); // removed.

            Assert.True(actual.Length < original.Length);
            Assert.Equal(original[bom.Length..], actual);

            // UTF-8 encoding BOM is...
            Assert.Equal(new byte[] { 0xEF, 0xBB, 0xBF }, bom);
        }

        [Fact]
        public void When_EncodingIsMatch_Then_ReturnsValueWithoutBom()
        {
            var encoding = Encoding.UTF8;
            var bom = encoding.GetPreamble();
            byte[] original = BuildWithBom("TEST文字列😄", encoding);

            byte[] actual = original.RemovePreamble(encoding);

            Assert.Equal(bom, original[..bom.Length]); // exists.
            Assert.NotEqual(bom, actual[..bom.Length]); // removed.

            Assert.True(actual.Length < original.Length);
            Assert.Equal(original[bom.Length..], actual);
        }

        [Fact]
        public void When_EncodingIsUnMatch_Then_ReturnsValueKeepingBom()
        {
            var encoding = Encoding.UTF8;
            var bom = encoding.GetPreamble();
            byte[] original = BuildWithBom("TEST文字列😄", encoding);

            byte[] actual = original.RemovePreamble(Encoding.UTF32);

            Assert.Equal(bom, original[..bom.Length]); // exists.
            Assert.Equal(bom, actual[..bom.Length]); // not removed.

            Assert.True(actual.Length == original.Length);
            Assert.Equal(original, actual);
        }

        [Fact]
        public void When_EncodingIsMatch_WithUtf16_Then_ReturnsValueWithoutBom()
        {
            var encoding = Encoding.GetEncoding("UTF-16");
            var bom = encoding.GetPreamble();
            byte[] original = BuildWithBom("TEST文字列😄", encoding);

            byte[] actual = original.RemovePreamble(encoding);

            Assert.Equal(bom, original[..bom.Length]); // exists.
            Assert.NotEqual(bom, actual[..bom.Length]); // removed.

            Assert.True(actual.Length < original.Length);
            Assert.Equal(original[bom.Length..], actual);

            // UTF-16 Little Endian encoding is...
            Assert.Equal(new byte[] { 0xFF, 0xFE }, bom);
        }
    }

    [Fact]
    public void When_BinaryReaderRead_Then_ReturnsValueKeepingBom()
    {
        var encoding = Encoding.UTF8;
        var bom = encoding.GetPreamble();
        byte[] original = BuildWithBom("TEST文字列😄", encoding);

        // Binary reader obediently reads from the preamble.
        byte[] actual;
        using (var stream = new MemoryStream(original))
        using (var reader = new BinaryReader(stream, Encoding.UTF8))
        {
            actual = reader.ReadBytes((int)stream.Length);
        }

        Assert.Equal(bom, original[..bom.Length]); // exists.
        Assert.Equal(bom, actual[..bom.Length]); // not removed.

        Assert.True(actual.Length == original.Length);
        Assert.Equal(original, actual);
    }

    [Fact]
    public void When_StreamReaderRead_Then_ReturnsValueWithoutBom()
    {
        var encoding = Encoding.UTF8;
        var bom = encoding.GetPreamble();
        byte[] original = BuildWithBom("TEST文字列😄", encoding);

        byte[] actual;
        using (var stream = new MemoryStream(original))
        using (var reader = new StreamReader(stream, encoding))
        {
            var line = reader.ReadToEnd();
            actual = encoding.GetBytes(line);
        }

        Assert.Equal(bom, original[..bom.Length]); // exists.
        Assert.NotEqual(bom, actual[..bom.Length]); // removed.

        Assert.True(actual.Length < original.Length);
        Assert.Equal(original[bom.Length..], actual);
    }

    [Fact]
    public void When_StreamReaderRead_WithSpan_Then_ReturnsValueWithoutBom()
    {
        var encoding = Encoding.UTF8;
        var bom = encoding.GetPreamble();
        byte[] original = BuildWithBom("TEST文字列😄", encoding);

        byte[] actual;
        using (var stream = new MemoryStream(original))
        using (var reader = new StreamReader(stream, encoding))
        {
            Span<char> buffer = stackalloc char[(int)stream.Length];
            var count = reader.ReadBlock(buffer);
            var text = buffer[..count].ToString();
            actual = encoding.GetBytes(text);
        }

        Assert.Equal(bom, original[..bom.Length]); // exists.
        Assert.NotEqual(bom, actual[..bom.Length]); // removed.

        Assert.True(actual.Length < original.Length);
        Assert.Equal(original[bom.Length..], actual);
    }

    private static byte[] BuildWithBom(string text, Encoding encoding)
    {
        using (var stream = new MemoryStream())
        {
            stream.Write(encoding.GetPreamble());
            stream.Write(encoding.GetBytes(text));
            stream.Flush();

            return stream.ToArray();
        }
    }
}
