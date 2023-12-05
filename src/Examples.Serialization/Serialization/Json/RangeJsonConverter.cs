using System.Text.Json;
using System.Text.Json.Serialization;
using Examples.Serialization.Text;

namespace Examples.Serialization.Json;

/// <summary>
/// Converts a <see cref="Range"/> or value to or from JSON.
/// </summary>
public class RangeJsonConverter : JsonConverter<Range>
{
    public override Range Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        => StringConverter.ToRange(reader.GetString());

    public override void Write(Utf8JsonWriter writer, Range value, JsonSerializerOptions options)
        => writer.WriteStringValue(value.ToString());

}

