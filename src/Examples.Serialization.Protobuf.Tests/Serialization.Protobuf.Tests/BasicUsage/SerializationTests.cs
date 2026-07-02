using Google.Protobuf;
using Google.Protobuf.WellKnownTypes;

namespace Examples.Serialization.Protobuf.Tests.BasicUsage;

/// <summary>
/// Tests for Google Protocol buffer Serialize.
/// </summary>
/// <seealso href="https://developers.google.com/protocol-buffers/docs/csharptutorial" />
public class SerializationTests
{
    [Fact]
    public void When_DeserializeAfterSerializing_Then_RestoresToOriginal()
    {
        var john = new Person
        {
            Id = 1234,
            Name = "John Doe",
            Email = "jdoe@example.com",
            Phones = { new Person.Types.PhoneNumber { Number = "555-4321", Type = Person.Types.PhoneType.Home } },
            LastUpdated = Timestamp.FromDateTimeOffset(DateTimeOffset.Parse("2022-11-21T01:23:45Z"))
        };

        // Serialize
        using var stream = new MemoryStream();
        using (var output = new CodedOutputStream(stream))
        {
            john.WriteTo(output);
        }
        byte[] serialized = stream.ToArray();

        // Deserialize
        Person deserialized = Person.Parser.ParseFrom(serialized);

        // Assert
        Assert.Equal(53, serialized.Length);

        Assert.Equal(john.Id, deserialized.Id);
        Assert.Equal(john.Name, deserialized.Name);
        Assert.Equal(john.Email, deserialized.Email);
        Assert.Equal(john.Phones.Count, deserialized.Phones.Count);
        Assert.Equal(john.Phones[0].Number, deserialized.Phones[0].Number);
        Assert.Equal(john.Phones[0].Type, deserialized.Phones[0].Type);
        Assert.Equal(john.LastUpdated, deserialized.LastUpdated);
    }
}
