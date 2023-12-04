using System.Xml;
using System.Xml.Linq;
using System.Xml.XPath;

namespace Examples.Serialization.Xml.Tests;

/// <summary>
/// Test to see the difference between <see cref="XDocument" /> and <see cref="XElement" />.
/// </summary>
/// <seealso href="https://docs.microsoft.com/ja-jp/dotnet/standard/linq/query-xdocument-vs-query-xelement" />
public partial class QueryXDocumentVsXElementTests
{
    [Fact]
    public async Task WhenCallingLoadAsync_UsingXDocument_ReturnsRootElement()
    {
        using var inner = new StringReader(ResponseXml);

        var settings = new XmlReaderSettings() { Async = true };
        using var reader = XmlReader.Create(inner, settings);

        // Load with XDocument.
        var xDoc = await XDocument.LoadAsync(reader, LoadOptions.None, default);
        xDoc.IsInstanceOf<XDocument>();

        var children = from x in xDoc.Elements()
                       select x;

        // When XDocument.Load, XDocument.Elements[0] is "weatherforecast".
        children.Count().Is(1);
        children.ElementAt(0).Name.Is("weatherforecast");

        // The root when executing an XPATH query is "/weatherforecast".
        xDoc.XPathSelectElements("/weatherforecast/pref/area").Count()
            .Is(16);

        xDoc.XPathSelectElement("/weatherforecast/pref/area[@id='石狩地方']")!.Parent!.Attribute("id")!.Value
            .Is("北海道");

        return;
    }

    [Fact]
    public async Task WhenUsingXElementLoad_ReturnsRootXElement()
    {
        using var inner = new StringReader(ResponseXml);

        var settings = new XmlReaderSettings() { Async = true };
        using var reader = XmlReader.Create(inner, settings);

        // Load with XElement.
        var xElem = await XElement.LoadAsync(reader, LoadOptions.None, default);
        xElem.IsInstanceOf<XElement>();
        xElem.Name.Is("weatherforecast");

        var children = from x in xElem.Elements()
                       select x;

        // When XElement.Load, XElement.Elements[0] is "title".
        children.Count().Is(7);
        children.ElementAt(0).Name.Is("title");

        // The root when executing an XPATH query is "/".
        xElem.XPathSelectElements("/pref/area").Count()
            .Is(16);
        xElem.XPathSelectElement("/pref/area[@id='石狩地方']")!.Parent!.Attribute("id")!.Value
            .Is("北海道");

        return;
    }

}
