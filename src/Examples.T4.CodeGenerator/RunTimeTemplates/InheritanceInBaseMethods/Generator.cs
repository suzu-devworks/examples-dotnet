namespace Examples.T4.CodeGenerator.RunTimeTemplates.InheritanceInBaseMethods;

public class Generator : ITemplateGenerator
{
    public void Generate(TemplateCodeWriter writer)
    {
        dynamic template = new MyTextTemplate1();
        writer.Write(template.TransformText());

        return;
    }

}
