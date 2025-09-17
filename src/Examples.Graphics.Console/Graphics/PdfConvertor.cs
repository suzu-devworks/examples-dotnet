using System.Runtime.InteropServices;
using PDFiumSharp;
using SkiaSharp;

namespace Examples.Graphics;

public class PdfConvertor
{
    private static readonly byte[] Header = [0x25, 0x50, 0x44, 0x46];

    static PdfConvertor()
    {
        // ???

        NativeLibrary.SetDllImportResolver(typeof(PDFium).Assembly, (name, assembly, searchPath) =>
        {
            IntPtr libHandle = IntPtr.Zero;

            if (name == "pdfium_x64" && RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            {
                if (!NativeLibrary.TryLoad("./runtimes/linux-x64/native/libpdfium.so",
                    assembly,
                    DllImportSearchPath.ApplicationDirectory,
                    out libHandle))
                {
                    NativeLibrary.TryLoad("libpdfium.so",
                     assembly,
                     DllImportSearchPath.ApplicationDirectory,
                     out libHandle);
                }
            }

            return libHandle;
        });
    }


    public static bool IsPdf(byte[] source)
    {
        return Header.AsSpan().SequenceEqual(source.AsSpan()[..Header.Length]);
    }

    public static ImageSource? ToPng(byte[] source)
    {
        if (!IsPdf(source)) { throw new ArgumentException($"{nameof(source)} is not PDF."); }

        using var doc = new PdfDocument(source);
        return ToImage(doc, SKEncodedImageFormat.Png);
    }

    public static ImageSource? ToJpeg(byte[] source, int quality)
    {
        if (!IsPdf(source)) { throw new ArgumentException($"{nameof(source)} is not PDF."); }

        using var doc = new PdfDocument(source);
        return ToImage(doc, SKEncodedImageFormat.Jpeg, quality);
    }

    public static ImageSource? ToImage(PdfDocument doc, SKEncodedImageFormat format, int quality = 100)
    {
        using var page = doc.Pages.FirstOrDefault();

        var width = (int?)page?.Width ?? 640;
        var height = (int?)page?.Height ?? 480;

        using var bitmap = new PDFiumBitmap(width, height, hasAlpha: true);

        bitmap.Fill(new PDFiumSharp.Types.FPDF_COLOR(255, 255, 255));
        page?.Render(bitmap);

        using var pixmap = new SKPixmap(
            new(bitmap.Width, bitmap.Height, SKColorType.Bgra8888),
            bitmap.Scan0,
            bitmap.Stride);

        using var encoded = pixmap.Encode(format, quality);

        return new(encoded.ToArray(), pixmap.Width, pixmap.Height);
    }

    public record ImageSource(byte[] Data, double Width, double Height);


}
