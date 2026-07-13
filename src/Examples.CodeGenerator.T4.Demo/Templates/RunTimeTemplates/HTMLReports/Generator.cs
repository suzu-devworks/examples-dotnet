namespace Examples.CodeGenerator.T4.Templates.RunTimeTemplates.HTMLReports;

public class Generator : ITemplateGenerator
{
    public Task GenerateAsync(ICodeWriter writer, CancellationToken cancellationToken = default)
    {
        MyWebPage page = new MyWebPage();
        var pageContent = page.TransformText();

        // Directory.CreateDirectory("temp");
        // File.WriteAllText("temp/outputPage.html", pageContent);

        writer.WriteLine(pageContent);

        return Task.CompletedTask;
    }
}
