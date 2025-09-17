namespace Examples.Graphics;

public static class ImageExtensions
{
    public static string GetMimeType(this byte[] source)
    {
        var found = images.FirstOrDefault(x => source.Length > x.MagicNumber.Length
                && x.MagicNumber.AsSpan().SequenceEqual(source.AsSpan()[..x.MagicNumber.Length])
                );
        if (found is not null)
        {
            return found.MimeType;
        }

        return "application/octet-stream";
    }

    private record Entry(string MimeType, byte[] MagicNumber);
    private static readonly IEnumerable<Entry> images = [
        new("image/jpeg", [0xff, 0xd8]),
        new("image/png", [0x89, 0x50, 0x4e, 0x47, 0x0d, 0x0a, 0x1a, 0x0a]),
        new("image/gif", [0x47, 0x49, 0x46, 0x38, 0x37, 0x61])

    ];

}

