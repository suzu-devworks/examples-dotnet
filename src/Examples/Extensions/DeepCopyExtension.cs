using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace Examples.Extensions
{
    public static class ClonableExtension
    {
        public static T DeepCopy<T>(this T target)
        {
            using var stream = new MemoryStream();
            var formatter = new BinaryFormatter();

#pragma warning disable SYSLIB0011

            formatter.Serialize(stream, target);
            stream.Position = 0;

            return (T)formatter.Deserialize(stream);

#pragma warning restore SYSLIB0011


        }
    }
}
