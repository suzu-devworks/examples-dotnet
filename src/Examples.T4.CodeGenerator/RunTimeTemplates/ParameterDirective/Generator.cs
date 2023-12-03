using Microsoft.VisualStudio.TextTemplating;

namespace Examples.T4.CodeGenerator.RunTimeTemplates.ParameterDirective;

public class Generator : ITemplateGenerator
{
    public void Generate(TemplateCodeWriter writer)
    {
        // var template = new MyRepeatWebPage();
        dynamic template = new MyRepeatWebPage();

        template.Session = new TextTemplatingSession();
        template.Session["TimesToRepeat"] = 5;
        // Add other parameter values to t.Session here.
        template.Initialize(); // Must call this to transfer values.
        writer.Write(template.TransformText());

        return;
    }

}
