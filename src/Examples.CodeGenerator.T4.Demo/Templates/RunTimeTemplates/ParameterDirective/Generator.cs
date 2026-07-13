namespace Examples.CodeGenerator.T4.Templates.RunTimeTemplates.ParameterDirective;

public class Generator : ITemplateGenerator
{
    public Task GenerateAsync(ICodeWriter writer, CancellationToken cancellationToken = default)
    {
        MyTemplate template = new MyTemplate();

        template.Session = new Microsoft.VisualStudio.TextTemplating.TextTemplatingSession();
        template.Session["TimesToRepeat"] = 5;
        // Add other parameter values to t.Session here.
        template.Initialize(); // Must call this to transfer values.

        var result = template.TransformText();

        writer.WriteLine(result);

        return Task.CompletedTask;
    }

}
