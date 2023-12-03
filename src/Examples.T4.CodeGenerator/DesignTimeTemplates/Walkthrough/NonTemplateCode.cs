using System.Xml;

namespace Examples.T4.CodeGenerator.Templates.DesignTimeTemplates.Walkthrough;

#nullable disable

public class NonTemplateCode
{
    public static void TestMethod()
    {
        XmlDocument xmlDocument = new();
        xmlDocument.Load(@"assets/exampleXml.xml");

        XmlNode catalog = xmlDocument.SelectSingleNode("catalog");
        foreach (XmlNode artist in catalog.SelectNodes("artist")!)
        {
            Console.WriteLine(artist.Attributes["name"].Value);
            foreach (XmlNode song in artist.SelectNodes("song"))
            {
                Console.WriteLine("   " + song.InnerText);
            }
        }
    }
}
