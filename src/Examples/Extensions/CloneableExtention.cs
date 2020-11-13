using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public static class ClonableExtension
{
    public static ICloneable DeepCopy(this ICloneable target)
    {
        var formatter = new BinaryFormatter();
        var result = null as ICloneable;
        using(var stream = new MemoryStream())
        {
            formatter.Serialize(stream, target);
            stream.Position = 0;
            result = formatter.Deserialize(stream) as ICloneable;
            stream.Close();
        }

        return result;
    }

}