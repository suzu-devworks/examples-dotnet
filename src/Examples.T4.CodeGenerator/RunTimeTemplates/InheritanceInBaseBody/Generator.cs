namespace Examples.T4.CodeGenerator.RunTimeTemplates.InheritanceInBaseBody;

public class Generator : ITemplateGenerator
{
    public void Generate(TemplateCodeWriter writer)
    {
        // var template = new DerivedTemplate1();
        dynamic template = new DerivedTemplate1();
        writer.Write(template.TransformText());

        return;
    }

}
