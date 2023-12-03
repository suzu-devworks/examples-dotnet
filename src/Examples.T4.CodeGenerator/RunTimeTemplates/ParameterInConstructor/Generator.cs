namespace Examples.T4.CodeGenerator.RunTimeTemplates.ParameterInConstructor;

public class Generator : ITemplateGenerator
{
    public void Generate(TemplateCodeWriter writer)
    {
        var data = new MyData
        {
            Items = new MyDataItem[] {
                new() { Name = "key1", Value = "val1"},
                new() { Name = "key2", Value = "val2"},
                new() { Name = "key3", Value = "val3"},
            }
        };

        dynamic template = new MyDataWebPage(data);
        writer.Write(template.TransformText());

        return;
    }
}
