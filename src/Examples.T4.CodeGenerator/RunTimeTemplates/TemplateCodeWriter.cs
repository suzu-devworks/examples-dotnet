namespace Examples.T4.CodeGenerator.RunTimeTemplates;

public class TemplateCodeWriter
{
    public string? OutputFilePath { get; init; }

    public void Write(string? contentText)
    {
        Console.WriteLine(contentText);
    }

}

