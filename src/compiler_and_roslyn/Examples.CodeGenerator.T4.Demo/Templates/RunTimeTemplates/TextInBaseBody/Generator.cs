namespace Examples.CodeGenerator.T4.Templates.RunTimeTemplates.TextInBaseBody;

public class Generator : ITemplateGenerator
{
    public Task GenerateAsync(ICodeWriter writer, CancellationToken cancellationToken = default)
    {
        DerivedTemplate1 t1 = new DerivedTemplate1();
        var result = t1.TransformText();

        writer.WriteLine(result);

        return Task.CompletedTask;
    }
}
