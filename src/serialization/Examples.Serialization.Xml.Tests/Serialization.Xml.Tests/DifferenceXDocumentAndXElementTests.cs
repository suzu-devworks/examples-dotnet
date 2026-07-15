using System.Xml;
using System.Xml.Linq;
using System.Xml.XPath;

namespace Examples.Serialization.Xml.Tests;

/// <summary>
/// Test to see the difference between <see cref="XDocument" /> and <see cref="XElement" />.
/// </summary>
/// <seealso href="https://docs.microsoft.com/ja-jp/dotnet/standard/linq/query-xdocument-vs-query-xelement" />
public partial class DifferenceXDocumentAndXElementTests
{
    public class XDocumentTests
    {
        private static Task<XDocument> LoadAsync(string xml, CancellationToken cancellationToken = default)
        {
            using var reader = XmlReader.Create(new StringReader(xml), new() { Async = true });
            return XDocument.LoadAsync(reader, LoadOptions.None, cancellationToken);
        }

        [Fact]
        public async Task When_UsingLinqToXml_Then_ReturnsEnumerableOfXElement()
        {
            var document = await LoadAsync(ResponseXml, TestContext.Current.CancellationToken);

            // Linq query to get the root element.
            var areas = from element in document.Elements()
                        from pref in element.Elements()
                        from area in pref.Elements()
                        where area.Name == "area" && area.Attribute("id")?.Value == "石狩地方"
                        select area;

            Assert.NotNull(areas);
            var actual = Assert.Single(areas);

            Assert.Equal("area", actual.Name);
            Assert.Equal("石狩地方", actual.Attribute("id")?.Value);
        }

        [Fact]
        public async Task When_UsingTraversalElements_Then_ReturnsEnumerableOfXElement()
        {
            var document = await LoadAsync(ResponseXml, TestContext.Current.CancellationToken);

            // Elements() method is used to get the child elements of the root element.
            var areas = document.Element("weatherforecast")
                ?.Elements("pref")
                ?.Elements("area")
                ?.Where(area => area.Attribute("id")?.Value == "石狩地方");

            Assert.NotNull(areas);
            var actual = Assert.Single(areas);

            Assert.Equal("area", actual.Name);
            Assert.Equal("石狩地方", actual.Attribute("id")?.Value);
        }

        [Fact]
        public async Task When_UsingXPathSelectElements_Then__ReturnsEnumerableOfXElement()
        {
            var document = await LoadAsync(ResponseXml, TestContext.Current.CancellationToken);

            // XPath from the root element.
            var areas = document.XPathSelectElements("/weatherforecast/pref/area[@id='石狩地方']");

            Assert.NotNull(areas);
            var actual = Assert.Single(areas);

            Assert.Equal("area", actual.Name);
            Assert.Equal("石狩地方", actual.Attribute("id")?.Value);
        }
    }

    public class XElementTests
    {
        private static Task<XElement> LoadAsync(string xml, CancellationToken cancellationToken = default)
        {
            using var reader = XmlReader.Create(new StringReader(xml), new() { Async = true });
            return XElement.LoadAsync(reader, LoadOptions.None, cancellationToken);
        }

        [Fact]
        public async Task When_UsingLinqToXml_Then_ReturnsEnumerableOfXElement()
        {
            var element = await LoadAsync(ResponseXml, TestContext.Current.CancellationToken);

            // Linq query to get the weatherforecast element.
            var areas = from pref in element.Elements()
                        from area in pref.Elements()
                        where area.Name == "area" && area.Attribute("id")?.Value == "石狩地方"
                        select area;

            Assert.NotNull(areas);
            var actual = Assert.Single(areas);

            Assert.Equal("area", actual.Name);
            Assert.Equal("石狩地方", actual.Attribute("id")?.Value);
        }

        [Fact]
        public async Task When_UsingTraversalElements_Then_ReturnsEnumerableOfXElement()
        {
            var element = await LoadAsync(ResponseXml, TestContext.Current.CancellationToken);

            // Elements() method is used to get the child elements of the weatherforecast element.
            var areas = element
                ?.Elements("pref")
                ?.Elements("area")
                ?.Where(area => area.Attribute("id")?.Value == "石狩地方");

            Assert.NotNull(areas);
            var actual = Assert.Single(areas);

            Assert.Equal("area", actual.Name);
            Assert.Equal("石狩地方", actual.Attribute("id")?.Value);
        }

        [Fact]
        public async Task When_UsingXPathSelectElements_Then__ReturnsEnumerableOfXElement()
        {
            var element = await LoadAsync(ResponseXml, TestContext.Current.CancellationToken);

            // XPath from the weatherforecast element.
            var areas = element.XPathSelectElements("pref/area[@id='石狩地方']");

            Assert.NotNull(areas);
            var actual = Assert.Single(areas);

            Assert.Equal("area", actual.Name);
            Assert.Equal("石狩地方", actual.Attribute("id")?.Value);
        }
    }
}
