using System.Xml;
using System.Xml.Linq;
using System.Xml.XPath;

namespace Examples.Serialization.Xml.Tests;

/// <summary>
/// Tests <see cref="UglyXmlTextReaderDecorator" /> methods.
/// </summary>
public class UglyXmlTextReaderDecoratorTests
{

    [Fact]
    public async Task WhenCallingNormalXmlReader_ThrowAnException()
    {
        using var inner = new StringReader(UglyXmlDocument1);

        var settings = new XmlReaderSettings
        {
            CheckCharacters = false,
            Async = true,
        };

        using var reader = XmlReader.Create(inner, settings);

        var ex = await Assert.ThrowsAsync<XmlException>(
            () => XElement.LoadAsync(reader, LoadOptions.None, default));

        return;
    }

    [Fact]
    public async Task WhenUsingUglyXmlTextReaderDecorator()
    {
        using var inner = new UglyXmlTextReaderDecorator(new StringReader(UglyXmlDocument1));

        var settings = new XmlReaderSettings
        {
            CheckCharacters = false,
            Async = true,
        };

        using var reader = XmlReader.Create(inner, settings);

        var document = await XElement.LoadAsync(reader, LoadOptions.None, default);

        document.XPathSelectElement(@"//*[starts-with(@name,'半角ｶﾅを含むノード')]")?.Value.Is("⭐️半角ｶﾅを含むノード");
        document.XPathSelectElements(@"//NG-GROUP/*").Count().Is(3);

        return;
    }


    private static readonly string UglyXmlDocument1 = """
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
