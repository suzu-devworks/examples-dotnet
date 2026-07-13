using System.Xml;
using Examples.CodeGenerator.T4.Generated.Walkthrough;

namespace Examples.CodeGenerator.T4.Templates.DesignTimeTemplates.Walkthrough;

public class CodeGeneratorTest
{
    public void TestMethod()
    {
        XmlDocument doc = new XmlDocument();
        doc.Load(Path.Combine(Project.GetRootPath(), @"Resources/exampleXml.xml"));

        Catalog catalog = new Catalog(doc);
        foreach (Artist artist in catalog.Artist)
        {
            System.Console.WriteLine(artist.Name);
            foreach (Song song in artist.Song)
            {
                System.Console.WriteLine("   " + song.Text);
            }
        }
    }
}
