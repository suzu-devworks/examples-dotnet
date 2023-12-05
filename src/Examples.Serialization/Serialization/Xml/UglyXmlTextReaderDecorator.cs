using Examples.Text;

namespace Examples.Serialization.Xml;

/// <summary>
/// A <see cref="TextReader" /> decorator for reading "stray" XML that does not comply with
/// the <see href="https://www.w3.org/TR/2006/REC-xml-20060816/">W3C Extensible Markup Language (XML) 1.0 (4th Edition)</see> and
/// The <see href="https://www.w3.org/TR/REC-xml-names/">Namespaces in XML 1.0 (3rd Edition)</see> Recommendations.
/// </summary>
/// <remarks>
/// <p>XML that exceeds the buffer size will probably fail.</p>
/// </remarks>
public class UglyXmlTextReaderDecorator : TextReader
{
    private readonly TextReader _inputReader;

    public UglyXmlTextReaderDecorator(TextReader input)
    {
        _inputReader = input;
    }

    public char ReplacementCharacter { get; set; } = '_';

    public static void ReplaceUglyCharacters(Span<char> buffer, int readCount, char replacement)
    {
        var isInTag = false;
        var isInAttribute = false;

        // Read from behind.
        for (var i = readCount; i >= 0; --i)
        {
            switch (buffer[i])
            {
                case '>':
                    isInTag = true;
                    break;
                case '<':
                    isInTag = false;
                    switch (buffer[i + 1])
                    {
                        case '/':
                            // end-tag </ >
                            if (IsUglyStartWith(buffer[i + 2], replacement))
                            {
                                buffer[i + 2] = replacement;
                            }
                            break;
                        case char next when next is '!' or '?':
                            // comment or DOCTYPE <! > or <? >
                            break;
                        case char next when IsUglyStartWith(next, replacement):
                            buffer[i + 1] = replacement;
                            break;
                        default:
                            break;
                    }
                    break;
                case '"' when isInTag:
                    isInAttribute = !isInAttribute;
                    break;
                case char c when char.IsControl(c) && (!char.IsWhiteSpace(c)):
                    buffer[i] = replacement;
                    break;
                case char c when JapaneseCharacters.IsHalfWidthKatakana(c) && isInTag && (!isInAttribute):
                    buffer[i] = replacement;
                    break;
                default:
                    break;
            }
        }

        return;
    }

    private static bool IsUglyStartWith(char @char, char replacement)
    {
        var result = @char switch
        {
            char c when replacement == c => false,
            char c when !char.IsLetter(c) => true,
            _ => false
        };

        return result;
    }


    public override void Close() => _inputReader.Close();
    public override int Peek() => _inputReader.Peek();
    public override int Read() => _inputReader.Read();

    public override int Read(char[] buffer, int index, int count)
    {
        var readCount = _inputReader.Read(buffer, index, count);
        ReplaceUglyCharacters(buffer.AsSpan(), readCount, ReplacementCharacter);

        return readCount;
    }

    public override int Read(Span<char> buffer) => _inputReader.Read(buffer);
    public override Task<int> ReadAsync(char[] buffer, int index, int count) => _inputReader.ReadAsync(buffer, index, count);

    public override async ValueTask<int> ReadAsync(Memory<char> buffer, CancellationToken cancellationToken = default)
    {
        var readCount = await _inputReader.ReadAsync(buffer, cancellationToken);
        ReplaceUglyCharacters(buffer.Span, readCount, ReplacementCharacter);

        return readCount;
    }

    public override int ReadBlock(char[] buffer, int index, int count) => _inputReader.ReadBlock(buffer, index, count);
    public override int ReadBlock(Span<char> buffer) => _inputReader.ReadBlock(buffer);
    public override Task<int> ReadBlockAsync(char[] buffer, int index, int count) => _inputReader.ReadBlockAsync(buffer, index, count);
    public override ValueTask<int> ReadBlockAsync(Memory<char> buffer, CancellationToken cancellationToken = default) => _inputReader.ReadBlockAsync(buffer, cancellationToken);
    public override string? ReadLine() => _inputReader.ReadLine();
    public override Task<string?> ReadLineAsync() => _inputReader.ReadLineAsync();
    public override string ReadToEnd() => _inputReader.ReadToEnd();
    public override Task<string> ReadToEndAsync() => _inputReader.ReadToEndAsync();

    protected override void Dispose(bool disposing) => _inputReader.Dispose();

}
