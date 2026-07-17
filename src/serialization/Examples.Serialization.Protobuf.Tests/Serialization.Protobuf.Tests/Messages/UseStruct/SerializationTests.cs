using System.Text.Json.Nodes;
using Examples.Serialization.Protobuf.Extensions;

namespace Examples.Serialization.Protobuf.Tests.Messages.UseStruct;

public class SerializationTests
{
    [Fact]
    public void When_DeserializeAfterSerializing_Then_RestoresOriginal()
    {
        var message = new UserMetadata()
        {
            UserId = "user-123",
            CustomAttributes = new Google.Protobuf.WellKnownTypes.Struct
            {
                Fields =
                {
                    { "role", Google.Protobuf.WellKnownTypes.Value.ForString("admin") },
                    { "age", Google.Protobuf.WellKnownTypes.Value.ForNumber(30) }
                }
            }
        };

        // Serialize
        byte[] serialized = message.Serialize();

        // Deserialize
        var deserialized = UserMetadata.Parser.ParseFrom(serialized);

        // Assert
        Assert.Equal(message.UserId, deserialized.UserId);
        Assert.Equal(message.CustomAttributes.Fields.Count, deserialized.CustomAttributes.Fields.Count);
        Assert.Equal(message.CustomAttributes.Fields["role"].StringValue, deserialized.CustomAttributes.Fields["role"].StringValue);
        Assert.Equal(message.CustomAttributes.Fields["age"].NumberValue, deserialized.CustomAttributes.Fields["age"].NumberValue);
    }

    [Fact]
    public void When_DeserializeAfterSerializing_WithJsonFormatter_Then_RestoresOriginal()
    {
        const string json = """
            {
                "user_id": "user-123",
                "custom_attributes": {
                    "role": "admin",
                    "age": 30
                }
            }
            """;

        var message = new UserMetadata()
        {
            UserId = "user-123",
            CustomAttributes = Google.Protobuf.JsonParser.Default
                .Parse<Google.Protobuf.WellKnownTypes.Struct>(json),
        };

        // Serialize
        byte[] serialized = message.Serialize();

        // Deserialize
        var deserialized = UserMetadata.Parser.ParseFrom(serialized);
        var deserializedJson = Google.Protobuf.JsonFormatter.Default.Format(deserialized.CustomAttributes);

        // Assert
        Assert.Equal(message.UserId, deserialized.UserId);

        var expectedNormalizedJson = JsonNode.Parse(json)?.ToJsonString();
        var actualNormalizedJson = JsonNode.Parse(deserializedJson)?.ToJsonString();
        Assert.Equal(expectedNormalizedJson, actualNormalizedJson);
    }
}
