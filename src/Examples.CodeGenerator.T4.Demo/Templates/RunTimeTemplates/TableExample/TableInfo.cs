namespace Examples.CodeGenerator.T4.Templates.RunTimeTemplates.TableExample;

public class TableInfo
{
    public required string Name { get; init; }

    public IEnumerable<ColumnInfo> Columns { get; init; } = [];

    public string? Description { get; init; }

}

public class ColumnInfo
{
    public required string Name { get; init; }

    public required string Type { get; init; }

    public bool IsPrimaryKey { get; set; }

    public bool NotNull { get; set; }

    public string? Description { get; set; }

}
