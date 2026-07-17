namespace Examples.CodeGenerator.T4.Templates.RunTimeTemplates.FragmentsInBaseMethods;

public class Generator : ITemplateGenerator
{
    public Task GenerateAsync(ICodeWriter writer, CancellationToken cancellationToken = default)
    {
        MyTextTemplate1 t1 = new MyTextTemplate1();
        var result = t1.TransformText();

        writer.WriteLine(result);

        return Task.CompletedTask;
    }
}
