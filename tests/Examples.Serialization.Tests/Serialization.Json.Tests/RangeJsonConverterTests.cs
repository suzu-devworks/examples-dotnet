using System.Text.Json;

namespace Examples.Serialization.Json.Tests;

/// <summary>
/// Tests <see cref="RangeJsonConverter" /> methods.
/// </summary>
public class RangeJsonConverterTests
{
    private readonly JsonSerializerOptions _options;

    public RangeJsonConverterTests()
    {
        _options = new();
        _options.Converters.Add(new RangeJsonConverter());
    }

    [Fact]
    public void WhenCallingsSerializeAndDeserialize()
    {
        Range expected = 0..10;
        var serializedJson = JsonSerializer.Serialize(expected, _options);

        var actual = JsonSerializer.Deserialize<Range>(serializedJson, _options);
        actual.Is(actual);
    }

}
