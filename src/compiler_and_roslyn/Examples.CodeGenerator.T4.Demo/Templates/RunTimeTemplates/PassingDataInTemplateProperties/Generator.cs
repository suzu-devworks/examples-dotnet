namespace Examples.CodeGenerator.T4.Templates.RunTimeTemplates.PassingDataInTemplateProperties;

public class Generator : ITemplateGenerator
{
    public Task GenerateAsync(ICodeWriter writer, CancellationToken cancellationToken = default)
    {
        MyWebPage page = new()
        {
            Title = "My Web Page",
            Content = "This is the content of my web page."
        };
        var pageContent = page.TransformText();

        writer.WriteLine(pageContent);

        return Task.CompletedTask;
    }
}
