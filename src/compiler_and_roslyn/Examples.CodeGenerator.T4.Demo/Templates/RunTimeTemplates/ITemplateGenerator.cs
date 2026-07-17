namespace Examples.CodeGenerator.T4.Templates.RunTimeTemplates;

public interface ITemplateGenerator
{
    Task GenerateAsync(ICodeWriter writer, CancellationToken cancellationToken = default);
}
