namespace Examples.Graphics;

public static class ImageExtensions
{
    public static string GetMimeType(this Stream source)
    {
        Span<byte> buffer = stackalloc byte[8];
        try
        {
            source.ReadExactly(buffer);
        }
        catch (EndOfStreamException)
        {
            // If fewer than 8 bytes are available, read what we can
            buffer = buffer[..source.Read(buffer)];
        }
        return buffer.ToArray().GetMimeType();
    }

    public static string GetMimeType(this byte[] source)
    {
        var found = Images.FirstOrDefault(x => x.MagicNumber.Length <= source.Length
                && x.MagicNumber.AsSpan().SequenceEqual(source.AsSpan()[..x.MagicNumber.Length])
                );
        if (found is not null)
        {
            return found.MimeType;
        }

        return "application/octet-stream";
    }

    private record Entry(string MimeType, byte[] MagicNumber);

    private static readonly IEnumerable<Entry> Images = [
        new("image/jpeg", [0xff, 0xd8]),
        new("image/png", [0x89, 0x50, 0x4e, 0x47, 0x0d, 0x0a, 0x1a, 0x0a]),
        new("image/gif", [0x47, 0x49, 0x46, 0x38, 0x39, 0x61]),
        new("image/gif", [0x47, 0x49, 0x46, 0x38, 0x37, 0x61]),
        new("image/webp", [0x52, 0x49, 0x46, 0x46]),
        new("image/bmp", [0x42, 0x4d]),
    ];

}
