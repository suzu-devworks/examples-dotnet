using System.Text.Json;
using Examples.Serialization.Json;

namespace Examples.Serialization.Tests.Json;

/// <summary>
/// Tests <see cref="RangeJsonConverter" /> methods.
/// </summary>
public class RangeJsonConverterTests
{
    private static readonly JsonSerializerOptions Options = new()
    {
        Converters = { new RangeJsonConverter() }
    };

    [Fact]
    public void When_DeserializeAfterSerializing_Then_RestoresToOriginal()
    {
        Range expected = 0..10;

        var serializedJson = JsonSerializer.Serialize(expected, Options);
        var actual = JsonSerializer.Deserialize<Range>(serializedJson, Options);

        Assert.Equal("\"0..10\"", serializedJson);
        Assert.Equal(expected, actual);
    }
}
