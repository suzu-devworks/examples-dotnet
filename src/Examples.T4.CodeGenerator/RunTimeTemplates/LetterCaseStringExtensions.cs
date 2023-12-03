namespace Examples.T4.CodeGenerator.RunTimeTemplates;

public static class LetterCaseStringExtensions
{
    public static string ToPascalCase(this string snakeCaseString)
        => string.IsNullOrEmpty(snakeCaseString) ?
            snakeCaseString : ToCamelCaseInner(snakeCaseString.AsSpan(), true);

    public static string ToCamelCase(this string snakeCaseString)
        => string.IsNullOrEmpty(snakeCaseString) ?
            snakeCaseString : ToCamelCaseInner(snakeCaseString.AsSpan(), false);

    private static string ToCamelCaseInner(ReadOnlySpan<char> source, bool isUpper)
    {
        var buffer = source.Length <= 100 ?
            stackalloc char[source.Length] : new char[source.Length];

        int written = 0;
        foreach (char c in source)
        {
            if (c == '_')
            {
                isUpper |= (written != 0);
                continue;
            }

            buffer[written++] = isUpper ? char.ToUpperInvariant(c) : char.ToLowerInvariant(c);
            isUpper = false;
        }
        return (written == 0) ? "" : new string(buffer[0..written]);
    }
}
