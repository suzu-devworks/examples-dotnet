using Examples.Serialization.Protobuf.Extensions;

namespace Examples.Serialization.Protobuf.Tests.Messages.UseNullables;

/// <summary>
/// Tests for nullable types.
/// </summary>
/// <seealso href="https://learn.microsoft.com/ja-jp/aspnet/core/grpc/protobuf#nullable-types" />
public class SerializationTests
{
    [Fact]
    public void When_DeserializeAfterSerializing_WithValue_Then_RestoresOriginal()
    {
        var original = new Person()
        {
            Id = 1,
            FirstName = "John",
            LastName = "Doe",
            Age = 30
        };

        // Serialize
        byte[] serialized = original.Serialize();

        // Deserialize
        var deserialized = Person.Parser.ParseFrom(serialized);

        // Assert
        Assert.Equal(17, serialized.Length);
        Assert.Equal(original.Id, deserialized.Id);
        Assert.Equal(original.FirstName, deserialized.FirstName);
        Assert.Equal(original.LastName, deserialized.LastName);
        Assert.NotNull(original.Age);
        Assert.NotNull(deserialized.Age);
        Assert.Equal(original.Age.Value, deserialized.Age.Value);
    }

    [Fact]
    public void When_DeserializeAfterSerializing_WithNull_Then_RestoresOriginal()
    {
        var original = new Person()
        {
            Id = 2,
            FirstName = "Jane",
            LastName = "Doe",
            Age = null,
        };

        // Serialize
        byte[] serialized = original.Serialize();

        // Deserialize
        var deserialized = Person.Parser.ParseFrom(serialized);

        // Assert
        Assert.Equal(13, serialized.Length);
        Assert.Equal(original.Id, deserialized.Id);
        Assert.Equal(original.FirstName, deserialized.FirstName);
        Assert.Equal(original.LastName, deserialized.LastName);
        Assert.Null(original.Age);
        Assert.Null(deserialized.Age);
    }
}
