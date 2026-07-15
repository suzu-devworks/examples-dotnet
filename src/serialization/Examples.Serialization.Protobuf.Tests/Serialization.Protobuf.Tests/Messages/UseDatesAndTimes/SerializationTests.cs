using Examples.Serialization.Protobuf.Extensions;

namespace Examples.Serialization.Protobuf.Tests.Messages.UseDatesAndTimes;

/// <summary>
/// Tests for date and time types.
/// </summary>
/// <seealso href="https://learn.microsoft.com/ja-jp/aspnet/core/grpc/protobuf#date-and-time-types" />
public class SerializationTests
{
    [Fact]
    public void When_DeserializeAfterSerializing_Then_RestoresOriginal()
    {
        var original = new Meeting()
        {
            Subject = "Team Meeting",
            Start = Google.Protobuf.WellKnownTypes.Timestamp.FromDateTimeOffset(DateTimeOffset.UtcNow),
            Duration = Google.Protobuf.WellKnownTypes.Duration.FromTimeSpan(TimeSpan.FromHours(7))
        };

        // Serialize
        byte[] serialized = original.Serialize();

        // Deserialize
        var deserialized = Meeting.Parser.ParseFrom(serialized);

        // Assert
        Assert.Equal(original.Subject, deserialized.Subject);
        Assert.Equal(original.Start, deserialized.Start);
        Assert.Equal(original.Duration, deserialized.Duration);
    }
}
