namespace Examples.T4.CodeGenerator.RunTimeTemplates.TableTemplate;

public class Generator : ITemplateGenerator
{
    public void Generate(TemplateCodeWriter writer)
    {
        var typeDef = new Dictionary<string, string>()
            {
                { "integer", "int" },
                { "varchar", "string" },
                { "date", "DateTime" },
            };

        var table = new TableInfo()
        {
            Name = "shain_master",
            Description = "社員マスタ",
            Columns = new[]
            {
                    new ColumnInfo() { Name = "shain_id", Type = "integer", IsPrimary = true, NotNull = true },
                    new ColumnInfo() { Name = "shain_name", Type = "varchar", NotNull = true },
                    new ColumnInfo() { Name = "address", Type = "varchar", Description = "住所" },
                    new ColumnInfo() { Name = "created_date", Type = "date" },
                }
        };
        ITemplate template = new TextTemplate(typeDef, "MyNameSpace", table);

        writer.Write(template.TransformText());

        return;
    }
}
