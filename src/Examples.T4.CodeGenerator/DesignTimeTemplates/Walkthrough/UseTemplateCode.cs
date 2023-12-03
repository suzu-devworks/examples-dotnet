namespace Examples.T4.CodeGenerator.DesignTimeTemplates.Walkthrough;

#nullable disable

public class UseTemplateCode
{
    public static void TestMethod()
    {
        ICatalog catalog = new Catalog();
        catalog.Load(@"assets/exampleXml.xml");
        foreach (IArtist artist in catalog.Artist)
        {
            Console.WriteLine(artist.Name);
            foreach (ISong song in artist.Song)
            {
                Console.WriteLine("   " + song.Text);
            }
        }
    }
}
