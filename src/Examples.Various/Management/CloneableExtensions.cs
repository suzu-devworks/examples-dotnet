using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;

//using System.Runtime.Serialization.Formatters.Binary;

namespace Examples.Management;

public static class CloneableExtensions
{
    public static T DeepCopy<T>(this T source)
        where T : notnull
    {
        using var stream = new MemoryStream();

        // SYSLIB0011
        //var formatter = new BinaryFormatter();
        // formatter.Serialize(stream, source);
        // stream.Position = 0;
        // return (T)formatter.Deserialize(stream);

        var option = new JsonSerializerOptions
        {
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
        };

        JsonSerializer.Serialize<T>(stream, source, option);
        stream.Position = 0;
        var result = JsonSerializer.Deserialize<T>(stream, option)
            ?? throw new InvalidOperationException("Deserialize result is null.");

        return result!;
    }

    public static T ShallowCopy<T>(this T source)
        where T : notnull
    {
        var method = source.GetType().GetMethod("MemberwiseClone", BindingFlags.NonPublic | BindingFlags.Instance);
        return (T)method!.Invoke(source, null)!;
    }

}

