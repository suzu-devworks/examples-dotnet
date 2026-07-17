using Examples.Serialization.Protobuf.Extensions;

namespace Examples.Serialization.Protobuf.Tests.Messages.UseOneOf;

/// <summary>
/// Tests for OneOf types.
/// </summary>
/// <seealso href="https://learn.microsoft.com/ja-jp/aspnet/core/grpc/protobuf#oneof" />
public class SerializationTests
{
    [Fact]
    public void When_DeserializeAfterSerializing_InOrderParsonError_Then_RestoresDataWasLastSet()
    {
        var message = new ResponseMessage()
        {
            // lost
            Person = new()
            {
                Name = "John Doe",
                Email = "jdoe@example.com"
            },
            Error = new()
            {
                Text = "Error CS0103 Person does not exist."
            },
        };

        // Serialize
        byte[] serialized = message.Serialize();

        // Deserialize
        var deserialized = ResponseMessage.Parser.ParseFrom(serialized);

        // Assert
        Assert.Equal(39, serialized.Length);
        Assert.Equal(ResponseMessage.ResultOneofCase.Error, deserialized.ResultCase);

        Assert.Null(deserialized.Person);

        Assert.NotNull(deserialized.Error);
        Assert.Equal(message.Error.Text, deserialized.Error.Text);
    }

    [Fact]
    public void When_DeserializeAfterSerializing_InOrderErrorParson_Then_RestoresDataWasLastSet()
    {
        var message = new ResponseMessage()
        {
            // lost
            Error = new()
            {
                Text = "Error CS0103 Person does not exist."
            },
            Person = new()
            {
                Name = "John Doe",
                Email = "jdoe@example.com"
            },
        };

        // Serialize
        byte[] serialized = message.Serialize();

        // Deserialize
        var deserialized = ResponseMessage.Parser.ParseFrom(serialized);

        // Assert
        Assert.Equal(30, serialized.Length);
        Assert.Equal(ResponseMessage.ResultOneofCase.Person, deserialized.ResultCase);

        Assert.NotNull(deserialized.Person);
        Assert.Equal(message.Person.Name, deserialized.Person.Name);
        Assert.Equal(message.Person.Email, deserialized.Person.Email);

        Assert.Null(deserialized.Error);
    }
}
