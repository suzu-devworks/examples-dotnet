using Google.Protobuf;
using Google.Protobuf.WellKnownTypes;

namespace Examples.Serialization.Proto.Tests.ProtocolBufferBasics;

/// <summary>
/// Tests for Google Protocol buffer Serialize.
/// </summary>
/// <seealso href="https://developers.google.com/protocol-buffers/docs/csharptutorial" />
public class BasicSerializationTests
{
    [Fact]
    public void WhenCallingsSerializeAndDeserialize()
    {
        // Arrange
        var john = new Person
        {
            Id = 1234,
            Name = "John Doe",
            Email = "jdoe@example.com",
            Phones = { new Person.Types.PhoneNumber { Number = "555-4321", Type = Person.Types.PhoneType.Home } },
            LastUpdated = Timestamp.FromDateTimeOffset(DateTimeOffset.Parse("2022-11-21T01:23:45Z"))
        };

        // Act (serialize)
        using var stream = new MemoryStream();
        using (var output = new CodedOutputStream(stream))
        {
            john.WriteTo(output);
        }
        var serialized = stream.ToArray();
        var base64 = Convert.ToBase64String(serialized);

        // Act (deserialize)
        var binaries = Convert.FromBase64String(base64);
        var deserialized = Person.Parser.ParseFrom(binaries);

        // Assert
        serialized.Length.Is(53);

        deserialized.Id.Is(john.Id);
        deserialized.Name.Is(john.Name);
        deserialized.Email.Is(john.Email);
        deserialized.Phones.Count.Is(john.Phones.Count);
        deserialized.Phones[0].Number.Is(john.Phones[0].Number);
        deserialized.Phones[0].Type.Is(john.Phones[0].Type);
        deserialized.LastUpdated.Is(john.LastUpdated);

        return;
    }

}
