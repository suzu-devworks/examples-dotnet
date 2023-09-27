using System;
using System.Linq;
using System.Text;

#nullable enable

namespace Examples.Fluency;

/// <summary>
/// Extension methods for values related to <see cref="Encoding" />.
/// </summary>
public static class EncodingExtensions
{
    /// <summary>
    /// Removes the preamble (BOM: Byte Order Mark) at the beginning of the byte array.
    /// </summary>
    /// <param name="source">A source byte array.</param>
    /// <param name="encoding">A <see cref="Encoding" /> instance. default is <see cref="Encoding.UTF8" />.</param>
    /// <returns>Removed values.</returns>
    public static byte[] RemovePreamble(this byte[] source, Encoding? encoding = null)
    {
        encoding ??= Encoding.UTF8;

        var bom = encoding.GetPreamble();
        if (source.AsSpan()[..bom.Length].SequenceEqual(bom))
        {
            return source[bom.Length..];
        }

        return source;
    }

}
