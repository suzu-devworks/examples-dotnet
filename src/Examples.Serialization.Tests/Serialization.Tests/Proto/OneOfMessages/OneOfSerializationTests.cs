using Google.Protobuf;

using static Examples.Serialization.Proto.OneOfMessages.ResponseMessage;

namespace Examples.Serialization.Proto.OneOfMessages.Tests;

/// <summary>
/// Tests for OneOf types.
/// </summary>
/// <seealso href="https://learn.microsoft.com/ja-jp/aspnet/core/grpc/protobuf?view=aspnetcore-7.0#oneof" />
public class OneOfSerializationTests
{
    [Fact]
    public void WhenCallingsSerializeAndDeserialize()
    {
        // Arrange
        var message = new ResponseMessage()
        {
            // lost.
            Error = new()
            {
                Text = "Error CS0103 Person does not exist."
            },
            // validate.
            Person = new()
            {
                Name = "John Doe",
                Email = "jdoe@example.com"
            }
        };

        // Act (serialze)
        using var stream = new MemoryStream();
        using (var output = new CodedOutputStream(stream))
        {
            message.WriteTo(output);
        }
        var serialized = stream.ToArray();
        var base64 = Convert.ToBase64String(serialized);

        // Act (deserialize)
        var binaries = Convert.FromBase64String(base64);
        var deserialized = ResponseMessage.Parser.ParseFrom(binaries);

        // Assert
        serialized.Length.Is(30);

        deserialized.ResultCase.Is(ResultOneofCase.Person);
        deserialized.Person.Name.Is(message.Person.Name);
        deserialized.Person.Email.Is(message.Person.Email);
        deserialized.Error.IsNull();

        return;
    }
}
