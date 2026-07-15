namespace Examples.Serialization.Protobuf.Extensions;

/// <summary>
/// Extension methods for <see cref="Google.Protobuf.IMessage{T}"/> .
/// </summary>
public static class MessageExtensions
{
    /// <summary>
    /// Serializes a <see cref="Google.Protobuf.IMessage{T}"/> to a byte array.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="original"></param>
    /// <returns></returns>
    public static byte[] Serialize<T>(this T original)
        where T : Google.Protobuf.IMessage<T>
    {
        using MemoryStream stream = new();
        using (var output = new Google.Protobuf.CodedOutputStream(stream))
        {
            original.WriteTo(output);
        }

        return stream.ToArray();
    }
}
