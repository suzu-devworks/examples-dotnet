namespace Examples.CodeGenerator.T4.Templates.RunTimeTemplates.PassingParametersInTheConstructor;

public class Generator : ITemplateGenerator
{
    public Task GenerateAsync(ICodeWriter writer, CancellationToken cancellationToken = default)
    {
        MyData data = new()
        {
            Items = [
                new() { Name = "key1", Value = "val1"},
                new() { Name = "key2", Value = "val2"},
                new() { Name = "key3", Value = "val3"},
            ]
        };

        MyWebPage page = new MyWebPage(data);
        var pageContent = page.TransformText();

        writer.WriteLine(pageContent);

        return Task.CompletedTask;
    }
}
