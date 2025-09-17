namespace Examples.T4.CodeGenerator.DesignTimeTemplates.Formatters;

public interface IMessagePackFormatter<T> where T : struct
{
    void Serialize(ref MessagePackWriter writer, T value, MessagePackSerializerOptions options);

    T Deserialize(ref MessagePackReader reader, MessagePackSerializerOptions options);

}

public class MessagePackSerializerOptions
{
}

public class MessagePackWriter
{

    public void WriteInt16(short value) { }
    public void WriteInt32(int value) { }
    public void WriteInt64(long value) { }
    public void WriteUInt16(ushort value) { }
    public void WriteUInt32(uint value) { }
    public void WriteUInt64(ulong value) { }
    public void WriteUInt8(byte value) { }
    public void WriteInt8(sbyte value) { }

}

public class MessagePackReader
{
    public short ReadInt16() { throw new NotSupportedException(); }
    public int ReadInt32() { throw new NotSupportedException(); }
    public long ReadInt64() { throw new NotSupportedException(); }
    public ushort ReadUInt16() { throw new NotSupportedException(); }
    public uint ReadUInt32() { throw new NotSupportedException(); }
    public ulong ReadUInt64() { throw new NotSupportedException(); }
    public byte ReadByte() { throw new NotSupportedException(); }
    public sbyte ReadSByte() { throw new NotSupportedException(); }

}

