using SkiaSharp;

namespace Examples.Graphics.SkiaSharp.Utilities;

public class FontResolver
{
    private static readonly string SansSerifFontPath = Path.Combine(
                                    Environment.GetFolderPath(Environment.SpecialFolder.UserProfile),
                                    @".local/share/fonts/NotoSansJP-Regular.otf");
    private static readonly string SerifFontPath = Path.Combine(
                                    Environment.GetFolderPath(Environment.SpecialFolder.UserProfile),
                                    @".local/share/fonts/NotoSerifJP-Regular.otf");

    public static SKTypeface FromFamilyName(string familyName)
    {
        var fontPath = familyName.Equals("sans-serif", StringComparison.OrdinalIgnoreCase)
            ? SansSerifFontPath
            : SerifFontPath;

        if (File.Exists(fontPath))
        {
            return SKTypeface.FromFile(fontPath);
        }
        else
        {
            Console.WriteLine($"Warning: Font file not found at {fontPath}. Using default font.");
            return SKTypeface.FromFamilyName(familyName);
        }
    }
}
