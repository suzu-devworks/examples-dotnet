namespace Examples.CodeGenerator.T4.Templates.RunTimeTemplates.TableExample;

public class Generator : ITemplateGenerator
{
    public Task GenerateAsync(ICodeWriter writer, CancellationToken cancellationToken = default)
    {
        TableInfo table = new()
        {
            Name = "employees",
            Description = "社員マスタ",
            Columns = [
                new() { Name = "id", Type = "integer", IsPrimaryKey = true, NotNull = true },
                new() { Name = "name", Type = "varchar", NotNull = true },
                new() { Name = "address", Type = "varchar", Description = "住所" },
                new() { Name = "created_date", Type = "timestamp" },
            ]
        };

        Dictionary<string, string> typeConvertors = new()
        {
            ["integer"] = "int",
            ["varchar"] = "string",
            ["timestamp"] = typeof(DateTime).Name,
        };

        var template = new TableTemplate(
            table,
            "Examples.CodeGenerator.T4.Templates.RunTimeTemplates.TableExample.Examples",
            typeConvertors);
        var result = template.TransformText();

        writer.WriteLine(result);

        return Task.CompletedTask;
    }
}
