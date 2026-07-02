using System.Text.Json;
using System.Text.Json.Serialization;
using Examples.Fluency.Extensions;

namespace Examples.Serialization.Json;

/// <summary>
/// Converts a <see cref="Range"/> or value to or from JSON.
/// </summary>
public class RangeJsonConverter : JsonConverter<Range>
{
    public override Range Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        => reader.GetString()?.ToRange()
            ?? throw new JsonException("Failed to convert JSON value to Range.");

    public override void Write(Utf8JsonWriter writer, Range value, JsonSerializerOptions options)
        => writer.WriteStringValue(value.ToString());
}
