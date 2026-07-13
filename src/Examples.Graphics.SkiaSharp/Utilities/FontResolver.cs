using SkiaSharp;

namespace Examples.Graphics.SkiaSharp.Utilities;

public class FontResolver
{
    private static readonly string SansSerifFontPath = Path.Combine(
                                    Environment.GetFolderPath(Environment.SpecialFolder.UserProfile),
                                    @".local/share/fonts/NotoSansJP[wght].ttf");
    private static readonly string SerifFontPath = Path.Combine(
                                    Environment.GetFolderPath(Environment.SpecialFolder.UserProfile),
                                    @".local/share/fonts/NotoSerifJP[wght].ttf");

    public static SKTypeface FromFamilyName(string familyName,
        SKFontStyleWeight weight = SKFontStyleWeight.Normal)
    {
        var fontPath = familyName.Equals("sans-serif", StringComparison.OrdinalIgnoreCase)
            ? SansSerifFontPath

            : SerifFontPath;

        if (File.Exists(fontPath))
        {
            var loadedTypeface = SKTypeface.FromFile(fontPath);

            if (!IsVariableFont(loadedTypeface))
            {
                // If the font is not variable, return the loaded typeface as is
                return loadedTypeface;
            }

            using var baseTypeface = loadedTypeface;
            return GetVariableFontAtWeight(baseTypeface, weight);
        }
        else
        {
            Console.WriteLine($"Warning: Font file not found at {fontPath}. Using default font.");
            return SKTypeface.FromFamilyName(familyName, SKFontStyleWeight.Normal, SKFontStyleWidth.Normal, SKFontStyleSlant.Upright);
        }
    }

    private static bool IsVariableFont(SKTypeface typeface)
    {
#if DEBUG
        foreach (var axis in typeface.VariationDesignParameters)
        {
            Console.WriteLine($"{axis.Tag}: [{axis.Min}, {axis.Default}, {axis.Max}] hidden={axis.IsHidden}");
        }

#endif
        return typeface.VariationDesignParameters.Any(axis => axis.Tag == SKFourByteTag.Parse("wght"));
    }

    private static SKTypeface GetVariableFontAtWeight(SKTypeface baseTypeface, SKFontStyleWeight weight)
    {
        var coord = new SKFontVariationPositionCoordinate
        {
            Axis = SKFourByteTag.Parse("wght"),
            Value = (float)weight,
        };

        var fontArgs = new SKFontArguments
        {
            VariationDesignPosition = new[] { coord }
        };

        return baseTypeface.Clone(fontArgs);
    }

}
