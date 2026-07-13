namespace Examples.CodeGenerator.T4.Templates.RunTimeTemplates;

public class ConsoleCodeWriter : ICodeWriter
{
    public void WriteLine(string? text = null)
    {
        System.Console.ForegroundColor = ConsoleColor.DarkCyan;
        System.Console.WriteLine(text);
        System.Console.ResetColor();
    }
}
