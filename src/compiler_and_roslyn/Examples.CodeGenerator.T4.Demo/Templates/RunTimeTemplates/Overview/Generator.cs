namespace Examples.CodeGenerator.T4.Templates.RunTimeTemplates.Overview;

public class Generator : ITemplateGenerator
{
    public Task GenerateAsync(ICodeWriter writer, CancellationToken cancellationToken = default)
    {
        var webResponseText = new MyTemplate().TransformText();
        writer.WriteLine(webResponseText);

        return Task.CompletedTask;
    }
}
