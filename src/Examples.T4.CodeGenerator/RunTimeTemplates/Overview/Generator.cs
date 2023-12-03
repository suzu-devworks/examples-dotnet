namespace Examples.T4.CodeGenerator.RunTimeTemplates.Overview;

public class Generator : ITemplateGenerator
{
    public void Generate(TemplateCodeWriter writer)
    {
        // var template = new MyWebPage();
        var type = Type.GetType(@"Examples.T4.CodeGenerator.RunTimeTemplates.MyWebPage");
        dynamic template = Activator.CreateInstance(type!)!;
        writer.Write(template.TransformText());

        return;
    }
}
