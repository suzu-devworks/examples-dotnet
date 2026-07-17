using System.Xml;
using System.Xml.Linq;
using System.Xml.XPath;
using Examples.Serialization.Xml;

namespace Examples.Serialization.Tests.Xml;

/// <summary>
/// Tests <see cref="UglyXmlTextReaderDecorator" /> methods.
/// </summary>
public class UglyXmlTextReaderDecoratorTests
{
    [Fact]
    public async Task When_ReadingUglyXml_Then_ThrowsException()
    {
        using var inner = new StringReader(UglyXml);

        var settings = new XmlReaderSettings
        {
            CheckCharacters = false,
            Async = true,
        };

        using var reader = XmlReader.Create(inner, settings);

        var exception = await Assert.ThrowsAsync<XmlException>(
            () => XElement.LoadAsync(reader, LoadOptions.None, TestContext.Current.CancellationToken));

        Assert.Contains("Name cannot begin with the '1' character, hexadecimal value 0x31.", exception.Message);
    }

    [Fact]
    public async Task When_ReadingUglyXml_WithUglyXmlTextReaderDecorator_Then_ReturnsXElement()
    {
        using var inner = new UglyXmlTextReaderDecorator(new StringReader(UglyXml));

        var settings = new XmlReaderSettings
        {
            CheckCharacters = false,
            Async = true,
        };

        using var reader = XmlReader.Create(inner, settings);

        var element = await XElement.LoadAsync(reader, LoadOptions.None, TestContext.Current.CancellationToken);

        var halfKanaNode = element.XPathSelectElement(@"//*[starts-with(@name,'半角ｶﾅを含むノード')]");
        var ngGroupElements = element.XPathSelectElements(@"//NG-GROUP/*");

        Assert.Equal("⭐️半角ｶﾅを含むノード", halfKanaNode?.Value);
        Assert.Equal(3, ngGroupElements.Count());
    }

    private static readonly string UglyXml = """
        <?xml version="1.0" encoding="utf-8"?>
        <DOC>
            <TEMPLATE>
                <OK-GROUP>
                    <node_normal name="normal">⭐️普通のノード</node_normal>
                    <__node-start-with-under name="under">⭐️_から始まるノード</__node-start-with-under>
                    <node漢字を含むノード name="漢字を含むノード">⭐️漢字を含むノード</node漢字を含むノード>
                    <漢字のみのノード name="漢字のみのノード">⭐️漢字のみのノード</漢字のみのノード>
                </OK-GROUP>
                <NG-GROUP>
                    <123数値から始まるノード name="123数値から始まるノード">⭐️数字から始まるノード</123数値から始まるノード>
                    <~記号から始まるノード name="記号から始まるノード">⭐️記号から始まるノード</~記号から始まるノード>
                    <node半角ｶﾅを含むノード name="半角ｶﾅを含むノード">⭐️半角ｶﾅを含むノード</node半角ｶﾅを含むノード>
                </NG-GROUP>
            </TEMPLATE>
        </DOC>
        """;
}
