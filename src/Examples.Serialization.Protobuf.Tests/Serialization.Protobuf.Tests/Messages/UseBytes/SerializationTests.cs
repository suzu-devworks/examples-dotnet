using Examples.Serialization.Protobuf.Extensions;
using Google.Protobuf;

namespace Examples.Serialization.Protobuf.Tests.Messages.UseBytes;

/// <summary>
/// Tests for bytes types.
/// </summary>
/// <seealso href="https://learn.microsoft.com/ja-jp/aspnet/core/grpc/protobuf#bytes" />
public class SerializationTests
{
    [Fact]
    public void When_DeserializeAfterSerializing_Then_RestoresOriginal()
    {
        var message = new PayloadResponse()
        {
            Data = ByteString.CopyFromUtf8("Hello, World!")
        };

        // Serialize
        byte[] serialized = message.Serialize();

        // Deserialize
        var deserialized = PayloadResponse.Parser.ParseFrom(serialized);

        // Assert
        Assert.Equal(15, serialized.Length);
        Assert.Equal(message.Data, deserialized.Data);
    }
}
